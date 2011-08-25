using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using WowTools.Core;

namespace WoWPacketViewer
{
    class ParserCompiler
    {
        public static Assembly CompileParser(string source, OpCodes opcode)
        {
            source = AddImpliedCode(source, opcode);
            using (CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp"))
            {
                CompilerParameters cp = new CompilerParameters();

                cp.GenerateInMemory = true;
                cp.TreatWarningsAsErrors = false;
                cp.GenerateExecutable = false;
                cp.ReferencedAssemblies.Add("System.dll");
                cp.ReferencedAssemblies.Add("System.Core.dll");
                cp.ReferencedAssemblies.Add("WowTools.Core.dll");
                cp.ReferencedAssemblies.Add("WoWPacketViewer.exe");

                CompilerResults cr = provider.CompileAssemblyFromSource(cp, source);

                if (cr.Errors.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (CompilerError ce in cr.Errors)
                        sb.AppendFormat("{0}", ce).AppendLine();
                    throw new Exception(sb.ToString());
                }

                return cr.CompiledAssembly;
            }
        }

        public static string AddImpliedCode(string source, OpCodes opcode)
        {
            bool knownOpcode = Enum.IsDefined(typeof (OpCodes), opcode);
            StringBuilder s = new StringBuilder(source.Length + 300);
            if (!source.Contains("using WowTools.Core"))
            {
                s.Append(
@"using System;
using System.Collections.Generic;
using WowTools.Core;").AppendLine().AppendLine();
            }
            int tab = 0;
            if (!source.Contains("namespace WoWPacketViewer"))
            {
                s.Append("namespace WoWPacketViewer").AppendLine();
                s.Append("{").AppendLine();
                tab++;
            }
            if (!source.Contains(" class "))
            {
                s.AppendFormat("    public class {0} : Parser", GetClassName(opcode)).AppendLine();
                s.Append("    {").AppendLine();
                tab++;
            }
            if(!source.Contains(" void "))  // hacky: assumes parser functions return void
            {
                if(!knownOpcode)
                {
                    s.AppendFormat("        [Parser((OpCodes){0})]", opcode).AppendLine();
                }
                string funcName = knownOpcode ? opcode.ToString() : "Handle" + (uint) opcode;
                s.AppendFormat("        public void {0}()", funcName).AppendLine();
                s.Append("        {").AppendLine();
                tab++;
            }
            var lines = source.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            foreach(string line in lines)
            {
                s.Append(new string(' ', 4*tab)); // add tabulation
                s.Append(line).AppendLine();
            }

            for (int i = tab - 1; i >= 0; i--)
            {
                s.Append(new string(' ', 4 * i)).Append("}").AppendLine();
            }

            using (var debug = new StreamWriter("LastParser.cs"))   // output complete code for possible debugging
            {
                debug.WriteLine(s.ToString());
            }

            return s.ToString();
        }

        public static string GetClassName(OpCodes opcode)
        {
            return ToCamel(opcode.ToString()) + "Parser";
        }

        /// <summary>
        /// Converts opcode name to camel case to be used in code.
        /// </summary>
        public static string ToCamel(string str)
        {
            var parts = str.Split('_');
            if (parts.Length < 2)
                return "Op" + str; // probably just a number

            for (int i = 1; i < parts.Length; i++)
            {
                parts[i] = parts[i][0] + parts[i].Substring(1).ToLower();
            }
            return String.Join("", parts, 1, parts.Length - 1);
        }

        // Source Loading / Replacing code follows

        public static readonly Dictionary<OpCodes, string> Sources = new Dictionary<OpCodes, string>();
        public static readonly Dictionary<OpCodes, string> SourceFiles = new Dictionary<OpCodes, string>();

        private static Regex AttrRegex = new Regex(@"^\s*\[\s*Parser\s*\(\s*OpCodes\.(\w+)\s*\)\s*\].*");
        private static Regex FuncNameRegex = new Regex(@"^(?:        |\t\t)\S.+? ((?:[SC])?MSG_[\w_]+)\(.*"); // requires proper tabulation
        private static Regex FuncEndRegex = new Regex(@"^(?:        |\t\t)\}");

        public static void LoadSources(string fileName)
        {
            var source = new StringBuilder();
            var parses = new List<OpCodes>();
            
            bool inFunction = false;
            foreach (string line in File.ReadLines(fileName))
            {
                // easy hacky way for finding ends of functions. tabulations must be correct.
                if (FuncEndRegex.IsMatch(line))
                {
                    AddSourceLine(source, line);
                    
                    string src = source.ToString();
                    foreach (var opcode in parses)
                    {
                        if (Sources.ContainsKey(opcode))
                            Console.WriteLine("Parser redefinition " + opcode);
                        Sources[opcode] = src;
                        SourceFiles[opcode] = fileName;
                    }

                    inFunction = false;
                    parses.Clear();
                    source.Clear();
                }
                OpCodes opc = CheckLine(line);
                if (opc != 0)
                {
                    parses.Add(opc);
                    inFunction = true;
                }
                if (inFunction)
                {
                    AddSourceLine(source, line);    // doesn't append the new line symbol
                    source.AppendLine();
                }
            }
        }

        public static bool TryReplaceSource(string source, OpCodes opcode)
        {
            if (!SourceFiles.ContainsKey(opcode))
                return false;

            return ReplaceSource(SourceFiles[opcode], source, opcode);
        }

        public static bool ReplaceSource(string fileName, string source, OpCodes targetOpcode)
        {
            bool inFunction = false;
            string[] oldContent = File.ReadAllLines(fileName);
            int funcStartLine = -1;
            bool replaceThis = false;
            bool replaced = false;
            string tempFile = fileName + ".new";
            using(var w = new StreamWriter(tempFile, false, Encoding.UTF8)) // TODO: keep encoding of the source file
            for (int l = 0; l < oldContent.Length; l++)
            {
                string line = oldContent[l];
                // easy hacky way for finding ends of functions. tabulations must be correct.
                if (inFunction && FuncEndRegex.IsMatch(line))
                {
                    if (replaceThis)
                    {
                        WriteAppendTabs(w, source);    // found and written
                        replaced = true;
                    }
                    else // write back the content of the funcion that ends here
                        for (int i = funcStartLine; i <= l; i++)
                            w.WriteLine(oldContent[i]);

                    inFunction = false;
                    replaceThis = false;
                    funcStartLine = -1;
                    continue;
                }
                OpCodes opc = CheckLine(line);
                if (opc != 0)
                {
                    if (!inFunction)
                        funcStartLine = l;
                    inFunction = true;
                    if (opc == targetOpcode)
                        replaceThis = true;
                }
                if (!inFunction)
                    w.WriteLine(line);
            }
            if (replaced)
            {
                File.Delete(fileName);
                File.Move(tempFile, fileName);
            }
            else
                File.Delete(tempFile);  // nothing replaced. delete the temp file

            return replaced;
        }

        private static OpCodes CheckLine(string line)
        {
            if (line.Contains("MSG_"))  // quick check to skip most lines
            {
                // potential parser defintion. investigate further
                Match m = AttrRegex.Match(line);
                if (!m.Success)
                    m = FuncNameRegex.Match(line);
                if (m.Success)
                {
                    var opcodeStr = m.Groups[1].Value;
                    OpCodes opcode;
                    if (Enum.TryParse<OpCodes>(opcodeStr, out opcode))
                        return opcode;
                    else
                        Console.WriteLine("ERROR: Found something that looks like an opcode but not in the enum " + opcodeStr);
                }
            }
            return 0;
        }

        private static void AddSourceLine(StringBuilder source, string line)
        {
            // for convenience and space saving remove 2 tabs that should be preceding all the parser function code
            if (line.StartsWith("        "))
                line = line.Substring(8);
            source.Append(line);
        }

        private static void WriteAppendTabs(StreamWriter w, string source)
        {
            foreach (string line in source.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
            {
                //if(!Regex.IsMatch(line, @"^\s*$"))
                    w.Write("        ");
                w.WriteLine(line);
            }
        }

        public static string GetSource(OpCodes opcode)
        {
            if (!Sources.ContainsKey(opcode))
                return "";
            return Sources[opcode];
        }
    }
}
