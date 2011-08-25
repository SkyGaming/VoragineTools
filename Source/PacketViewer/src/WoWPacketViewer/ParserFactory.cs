using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using WowTools.Core;

namespace WoWPacketViewer
{
    public class ParserFactory
    {
        private static DateTime LastUpdated = DateTime.Now;
        private static readonly Dictionary<OpCodes, Type> Parsers = new Dictionary<OpCodes, Type>();
        private static readonly Dictionary<OpCodes, MethodInfo> MethodParsers = new Dictionary<OpCodes, MethodInfo>();
        private static readonly Parser UnknownParser = new UnknownPacketParser();

        public static void ReInit()
        {
            Parsers.Clear();
            MethodParsers.Clear();
            Init();
        }

        public static void Init()
        {
            LoadAssembly(Assembly.GetCallingAssembly());

            if (Directory.Exists("parsers"))
            {
                foreach (string file in Directory.GetFiles("parsers", "*.dll", SearchOption.TopDirectoryOnly))
                {
                    try
                    {
                        if (File.GetLastWriteTime(file) < LastUpdated)
                            continue;
                        Assembly assembly = Assembly.LoadFile(Path.GetFullPath(file));
                        LoadAssembly(assembly);
                    }
                    catch
                    {
                    }
                }

                var extensions = new string[] { "*.cs", "*.vb" };

                foreach (var ext in extensions)
                {
                    foreach (string file in Directory.GetFiles("parsers", ext, SearchOption.TopDirectoryOnly))
                    {
                        try
                        {
                            Assembly assembly = CompileParser(file);
                            LoadAssembly(assembly);
                        }
                        catch
                        {
                        }
                    }
                }

                foreach (var dir in Directory.GetDirectories("parsers"))
                {
                    foreach (var ext in extensions)
                    {
                        var files = Directory.GetFiles(dir, ext, SearchOption.TopDirectoryOnly);

                        if (files.Length != 0)
                        {
                            Assembly assembly = CompileParser(files);
                            LoadAssembly(assembly);
                        }
                    }
                }
            }
            LastUpdated = DateTime.Now;
        }

        private static string GetLanguageFromExtension(string file)
        {
            return CodeDomProvider.GetLanguageFromExtension(Path.GetExtension(file));
        }

        private static Assembly CompileParser(params string[] files)
        {
            using (CodeDomProvider provider = CodeDomProvider.CreateProvider(GetLanguageFromExtension(files[0])))
            {
                CompilerParameters cp = new CompilerParameters();

                cp.GenerateInMemory = true;
                cp.TreatWarningsAsErrors = false;
                cp.GenerateExecutable = false;
                cp.ReferencedAssemblies.Add("System.dll");
                cp.ReferencedAssemblies.Add("System.Core.dll");
                cp.ReferencedAssemblies.Add("WowTools.Core.dll");

                CompilerResults cr = provider.CompileAssemblyFromFile(cp, files);

                if (cr.Errors.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (CompilerError ce in cr.Errors)
                        sb.AppendFormat("{0}", ce.ToString()).AppendLine();
                    MessageBox.Show(sb.ToString(), "Compile error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return cr.CompiledAssembly;
            }
        }

        private static void LoadAssembly(Assembly assembly, bool initialization = true)
        {
            foreach (Type type in assembly.GetTypes())
            {
                //if (type.IsSubclassOf(typeof(Parser))) // check disabled to support static parser classes
                {
                    var attributes = (ParserAttribute[])type.GetCustomAttributes(typeof(ParserAttribute), true);
                    foreach (ParserAttribute attribute in attributes)
                    {
                        if (initialization)
                            EnsureUnique(attribute.Code);
                        Parsers[attribute.Code] = type;
                    }

                    foreach (MethodInfo mi in type.GetMethods(BindingFlags.DeclaredOnly 
                        | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        attributes = (ParserAttribute[])mi.GetCustomAttributes(typeof(ParserAttribute), true);
                        foreach (ParserAttribute attribute in attributes)
                        {
                            if (initialization)
                                EnsureUnique(attribute.Code);
                            MethodParsers[attribute.Code] = mi;
                        }

                        OpCodes opcode;
                        if (Enum.TryParse(mi.Name, true, out opcode))
                        {
                            if (initialization)
                                EnsureUnique(opcode);
                            MethodParsers[opcode] = mi;
                        }
                    }
                }
            }
        }

        public static Parser CreateParser(Packet packet)
        {
            Type type;
            if (Parsers.TryGetValue(packet.Code, out type))
            {
                var parser = (Parser) Activator.CreateInstance(type);
                parser.Initialize(packet);
                parser.Parse();
                parser.CheckPacket();
                return parser;
            }
            MethodInfo mi;
            if (MethodParsers.TryGetValue(packet.Code, out mi))
            {
                Type createdType = mi.IsStatic ? typeof (Parser) : mi.DeclaringType;
                var parserObj = (Parser) Activator.CreateInstance(createdType);
                parserObj.Initialize(packet);
                var args = new object[mi.GetParameters().Length];
                if (args.Length > 0)
                    args[0] = parserObj; // pass the Parser object as a parameter for compatibility
                try
                {
                    mi.Invoke(parserObj, args);
                }
                catch (Exception e)
                {
                    if (e.InnerException != null)
                        e = e.InnerException;
                    parserObj.WriteLine("ERROR: Parsing failed with exception " + e);
                }
                parserObj.CheckPacket();
                return parserObj;
            }
            return UnknownParser;
        }

        private static void EnsureUnique(OpCodes opcode)
        {
            if (HasParser(opcode))
                MessageBox.Show("Parser redefined for " + opcode);
        }

        public static bool HasParser(OpCodes opcode)
        {
            return Parsers.ContainsKey(opcode) || MethodParsers.ContainsKey(opcode);
        }

        public static void DefineParser(string source, OpCodes opcode)
        {
            Assembly asm = ParserCompiler.CompileParser(source, opcode);
            LoadAssembly(asm, false);
        }
    }
}
