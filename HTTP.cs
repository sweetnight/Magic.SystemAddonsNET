namespace Magic.SystemAddonsNET
{
    public class HTTP : IDisposable
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task SendFilesAsync(string targetURL, List<string> filenames, Dictionary<string, string> anotherData)
        {
            var multipartContent = new MultipartFormDataContent();

            // Add the key-value pairs to the multipart form data content
            foreach (var entry in anotherData)
            {
                multipartContent.Add(new StringContent(entry.Value), entry.Key);
            }

            // Add the files to the multipart form data content
            foreach (string fileName in filenames)
            {
                var fileContent = new ByteArrayContent(File.ReadAllBytes(fileName));
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                multipartContent.Add(fileContent, "the_files[]", Path.GetFileName(fileName));
            }

            // Send the POST request
            HttpResponseMessage response = await client.PostAsync(targetURL, multipartContent);

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            // Optionally, read the response
            // string responseBody = await response.Content.ReadAsStringAsync();
        }

        /***
         * Method ini harus dipanggil di dalam async method jika ditunggu hasilnya menggunakan await, karena ini asynchronous.
         * Atau menggunakan Task.Run(async() => {}) method, ini berarti beda thread.
         * 
         * Kalau tidak ditunggui hasilnya, nggak perlu await, nggak perlu async.
         */
        public async Task<string?> Get(string url)
        {
            string response = "";

            HttpResponseMessage checkResponse;

            try
            {
                checkResponse = await client.GetAsync(url);
            }
            catch
            {
                return null;
            }

            response = await checkResponse.Content.ReadAsStringAsync(); // json

            return response;
        } // end of function

        /***
         * Method ini harus dipanggil di dalam async method jika ditunggu hasilnya menggunakan await, karena ini asynchronous.
         * Atau menggunakan Task.Run(async() => {}) method, ini berarti beda thread.
         * 
         * Kalau tidak ditunggui hasilnya, nggak perlu await, nggak perlu async.
         */
        public async Task<string?> Post(string url, Dictionary<string, string> values)
        {
            string response = "";

            FormUrlEncodedContent content = new FormUrlEncodedContent(values);

            HttpResponseMessage checkResponse;

            try
            {
                checkResponse = await client.PostAsync(url, content);
            }
            catch
            {
                return null;
            }

            response = await checkResponse.Content.ReadAsStringAsync(); // json

            return response;
        } // end of function

        private readonly HttpClient _httpClient;

        public event Action<int>? DownloadProgressChanged;

        public HTTP()
        {
            // Menggunakan native HTTP client handler
            var handler = new HttpClientHandler
            {
                // Konfigurasi tambahan bisa diterapkan di sini jika diperlukan
            };

            _httpClient = new HttpClient(handler);
        }

        public async Task DownloadFileAsync(string requestUri, string destinationFilePath)
        {
            using (HttpResponseMessage response = await _httpClient.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                long totalBytes = response.Content.Headers.ContentLength ?? -1L;
                long downloadedBytes = 0L;

                using (var contentStream = await response.Content.ReadAsStreamAsync())
                using (var fileStream = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                {
                    var buffer = new byte[8192];
                    int bytesRead;

                    while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await fileStream.WriteAsync(buffer, 0, bytesRead);
                        downloadedBytes += bytesRead;

                        if (totalBytes > 0 && DownloadProgressChanged != null)
                        {
                            int progress = (int)((downloadedBytes / (double)totalBytes) * 100);
                            DownloadProgressChanged(progress);
                        }
                    }
                }
            }
        } // end of method # managed by chat gpt

        public void Dispose()
        {

            _httpClient.Dispose();

        } // end of method

    } // end of class
} // end of method
