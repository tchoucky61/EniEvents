using System.Web;
using System.Web.Optimization;

namespace EniEvents
{
    public class BundleConfig
    {
        // Pour plus d'informations sur le regroupement, visitez http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/js/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Content/js/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                        "~/Content/js/custom*"));

            bundles.Add(new ScriptBundle("~/bundles/materialize").Include(
                        "~/Content/lib/materialize/js/materialize.min.js"));

            // Utilisez la version de développement de Modernizr pour le développement et l'apprentissage. Puis, une fois
            // prêt pour la production, utilisez l'outil de génération (bluid) sur http://modernizr.com pour choisir uniquement les tests dont vous avez besoin.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/js/modernizr-*"));

            bundles.Add(new StyleBundle("~/bundles/styles").Include(
                        "~/Content/css/styles.css"));

            bundles.Add(new StyleBundle("~/bundles/materializeStyles").Include(
                        "~/Content/lib/materialize/css/materialize.min.css"));
        }
    }
}
