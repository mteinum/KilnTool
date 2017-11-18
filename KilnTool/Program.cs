using System;
using System.IO;
using KilnTool.Kiln;
using System.Diagnostics;

namespace KilnTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new KilnApi();

            var projects = p.GetProjects();

            CreateDirectoryStructure(projects);
            CloneMissingProjects(projects);

            Console.ReadLine();
        }

        public static string ProjectDirectory(Kiln.KilnProject project)
        {
            var devRoot = System.Configuration.ConfigurationManager.AppSettings["local.directory"];

            return Path.Combine(devRoot, project.sName);
        }

        public static string RepositoryDirectory(Kiln.KilnProject project, Kiln.KilnRepository repository)
        {
            return Path.Combine(ProjectDirectory(project), repository.sName);
        }

        public static void CreateMissingDirectory(string folder)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
        }

        public static void CreateDirectoryStructure(KilnProject[] projects)
        {
            foreach (var project in projects)
            {
                CreateMissingDirectory(ProjectDirectory(project));

                foreach (var repo in project.repoGroups[0].repos)
                {
                    CreateMissingDirectory(RepositoryDirectory(project, repo));
                }
            }
        }

        public static void CloneMissingProjects(KilnProject[] projects)
        {
            foreach (var project in projects)
            {
                foreach (var repository in project.repoGroups[0].repos)
                {
                    CloneMissingRepository(project, repository);
                }
            }
        }

        public static void CloneMissingRepository(KilnProject project, KilnRepository repository)
        {
            var hgDirectory = Path.Combine(RepositoryDirectory(project, repository), ".hg");

            if (Directory.Exists(hgDirectory))
                return;

            HgClone(repository.sHgUrl, RepositoryDirectory(project, repository));
        }

        public static void HgClone(string url, string directory)
        {
            Hg(directory, $"clone --verbose {url} \"{directory}\"");
        }

        public static int Hg(string directory, string arguments)
        {
            var start = new ProcessStartInfo("hg.exe")
            {
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            Console.WriteLine($"Start clone: {start.Arguments}. Directory exists={Directory.Exists(directory)}");

            using (Process process = new Process { StartInfo = start })
            {
                process.OutputDataReceived += (s, e) => Console.WriteLine(e.Data);
                process.ErrorDataReceived += (s, e) => Console.WriteLine(e.Data);

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));

                Console.WriteLine($"Complete. Exit Code: {process.ExitCode}");

                return process.ExitCode;
            }
        }
    }
}
