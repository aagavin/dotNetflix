using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;

namespace dotNetflix.Models
{
	class BucketAccess
	{

		private string _projectId;
		private StorageClient _storage;
		private string _bucketName;


		/// <summary>
		/// Creates an instance of bucket access
		/// </summary>
		public BucketAccess()
		{
			string path = "./COMP-306.json";
			System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

			this._projectId = "comp-306";
			this._storage = StorageClient.Create();
			this._bucketName = this._projectId + "comp-306.appspot.com";

		}


		/// <summary>
		/// Uploads the a file to data storage
		/// and returns a url for public access
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="fileType"></param>
		/// <param name="file"></param>
		/// <returns></returns>
		public async Task<string> UpdateFile(string fileName, string fileType, Stream file)
		{

			try
			{
				var obj1 = await this._storage.UploadObjectAsync("comp-306.appspot.com", fileName, fileType, file, new UploadObjectOptions()
				{
					PredefinedAcl = PredefinedObjectAcl.PublicRead
				});

				return obj1.MediaLink;
			}
			catch (Google.GoogleApiException e)
			{

				return e.Error.Message;
			}

		}

	}

}