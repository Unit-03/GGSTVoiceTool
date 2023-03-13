using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

//using Octokit;

namespace GGSTVoiceTool
{
    public static class DownloadManager
    {
        #region Constants

        // I'll be honest I don't understand web stuff, apparently I need this to download things though so here it is!!
        private const string USER_AGENT = "Mozilla/5.0 (Windows; U; Windows NT 6.1; rv:2.2) Gecko/20110201";

        // head...
        private const string METHOD_HEAD = "HEAD"; // This makes it download the header data
        private const string METHOD_GET  = "GET";  // This makes it download the payload data

        #endregion

        #region Methods

        public static async Task<(bool, Version)> HasNewRelease()
        {
			//GitHubClient client = new GitHubClient(new ProductHeaderValue(Paths.RepoName), 
			//                                       new Uri($"{Paths.GitHubURL}/{Paths.GitHubUser}"));

			//var releases = await client.Repository.Release.GetAll(Paths.GitHubUser, Paths.RepoName);

			//if (releases.Count == 0)
			//    return (false, default);

			//try
			//{
			//    SemVersion version = new SemVersion(releases[0].TagName);

			//    bool isNewer = version > SemVersion.Current;

			//    return (isNewer, version);
			//}
			//catch
			//{
			//    return (false, default);
			//}

			return (true, Version.Current);
        }

        public static async Task<Stream> DownloadAsset(string url, string cache)
        {
            bool cached = File.Exists(cache);

            if (!cached && Settings.UseCache == true)
            {
                string cacheDir = Path.GetDirectoryName(cache);

                if (!Directory.Exists(cacheDir))
                    Directory.CreateDirectory(cacheDir);

                using WebClient client = new WebClient();
                await client.DownloadFileTaskAsync(new Uri(url), cache);
                cached = true;
            }

            if (cached)
                return File.OpenRead(cache);

            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.UserAgent = USER_AGENT;
            request.Method = METHOD_GET;

            using WebResponse response = await request.GetResponseAsync();
            return response.GetResponseStream();
        }

        public static async Task<WebResponse> GetHeaderData(Character charId, Language langId)
        {
            Paths.Properties.Character = charId;
            Paths.Properties.Language  = langId;

            HttpWebRequest request = WebRequest.CreateHttp(Paths.Assets.Voice.URL);
            request.UserAgent = USER_AGENT;
            request.Method    = METHOD_HEAD;

            return await request.GetResponseAsync();
        }

        public static async Task<long> GetDownloadSize(string url, string cache)
        {
            if (File.Exists(cache))
                return 0;

            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.UserAgent = USER_AGENT;
            request.Method    = METHOD_HEAD;

            using WebResponse response = await request.GetResponseAsync();
            return response.ContentLength;
        }

        public static async Task<long> GetDownloadSize(PatchInfo patchInfo)
        {
            long totalSize = 0;

            foreach (var patch in patchInfo)
            {
                Paths.Properties.Character = patch.Character;
                Paths.Properties.Language  = patch.UseLang;

                totalSize += await GetDownloadSize(Paths.Assets.Voice.URL, Paths.Assets.Voice.Cache);
            }

            return totalSize;
        }

        #endregion
    }
}
