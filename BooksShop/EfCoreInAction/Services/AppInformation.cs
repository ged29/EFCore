using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Versioning;

namespace EfCoreInAction.Services
{
    public class AppInformation
    {
        public AppInformation(string gitBranchName)
        {
            GitBranchName = gitBranchName;
        }

        public string GitBranchName { get; private set; }

        public IEnumerable<Tuple<string, string>> GetAssembliesInfo()
        {
            AssemblyName efCore = typeof(DbContext).GetTypeInfo().Assembly.GetName();
            yield return new Tuple<string, string>(efCore.Name, efCore.Version.ToString());

            AssemblyName aspNetCore = typeof(WebHostBuilder).GetTypeInfo().Assembly.GetName();
            yield return new Tuple<string, string>(aspNetCore.Name, aspNetCore.Version.ToString());

            Assembly netCoreAssembly = typeof(Program).GetTypeInfo().Assembly;
            yield return new Tuple<string, string>("Targeted .NET Core", netCoreAssembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName);
        }
    }
}