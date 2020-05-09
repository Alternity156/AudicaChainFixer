using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;
using MelonLoader;

[assembly: AssemblyTitle(ChainFixer.BuildInfo.Name)]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(ChainFixer.BuildInfo.Company)]
[assembly: AssemblyProduct(ChainFixer.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + ChainFixer.BuildInfo.Author)]
[assembly: AssemblyTrademark(ChainFixer.BuildInfo.Company)]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
//[assembly: Guid("")]
[assembly: AssemblyVersion(ChainFixer.BuildInfo.Version)]
[assembly: AssemblyFileVersion(ChainFixer.BuildInfo.Version)]
[assembly: NeutralResourcesLanguage("en")]
[assembly: MelonModInfo(typeof(ChainFixer.ChainFixer), ChainFixer.BuildInfo.Name, ChainFixer.BuildInfo.Version, ChainFixer.BuildInfo.Author, ChainFixer.BuildInfo.DownloadLink)]


// Create and Setup a MelonModGame to mark a Mod as Universal or Compatible with specific Games.
// If no MelonModGameAttribute is found or any of the Values for any MelonModGame on the Mod is null or empty it will be assumed the Mod is Universal.
// Values for MelonModGame can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonModGame(null, null)]