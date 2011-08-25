#################
# 	 Ascent     #
#################

Install this program.
With default settings.
In Complier settings choose your complier version

2005 / 2008 / 2010.

After that press right click on My Computer.
// Windows 7 Users

Press Advanced system settings, than press Environment Variables...
At User variables for YourUsername
Press New...

and Write:
In Variable: BOOST_ROOT
and in Value: C:\Program Files\boost\boost_1_46_1\
//comment for value, this is example, you need to put path to that folder, where you installed boost program.

and press ok.

Now go to System variables press new...
In Variable write: BOOST_ROOT
and in Value: C:\Program Files\boost
press ok.

Than you can run cmake.

Kind Regards
 Ascent Team