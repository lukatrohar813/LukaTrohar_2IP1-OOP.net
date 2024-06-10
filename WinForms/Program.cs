using DAL.Api;
using DAL.Repos;
using System;
using System.Windows.Forms;
using WinForms.Forms;

namespace WinForms
{
    internal static class Program
    {
        private static IFileRepository _repository;
        private static IApi _api;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            InitializeDependencies();

            ApplicationConfiguration.Initialize();

            Application.Run(DetermineStartupForm());
        }

        private static void InitializeDependencies()
        {
            _repository = RepositoryFactory.GetRepository();
            _api = ApiFactory.GetApi();
        }

        private static Form DetermineStartupForm()
        {
            if (!_repository.DoSettingsExist())
            {
                return new InitialSettingsForm(_repository, _api);
            }
            else if (!_repository.DoesSelectedTeamExist())
            {
                return new InitialTeamSelectForm(_repository, _api);
            }
            else
            {
                return new MainForm(_repository, _api);
            }
        }
    }
}