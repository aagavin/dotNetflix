using System;
using System.Text;
using Google.Cloud.Storage.V1;

namespace dotNetflix.Models
{
	class BucketAccess
	{

		private string _projectId;
		private StorageClient _storage;
		private string _bucketName;

		public BucketAccess()
		{
			string path = "./COMP-306.json";
			System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

			this._projectId = "comp-306";
			this._storage = StorageClient.Create();
			this._bucketName = this._projectId + "comp-306.appspot.com";

		}


		public string UpdateFile(string fileName, string fileType, System.IO.MemoryStream file)
		{

			try
			{

				var obj1 = this._storage.UploadObject("comp-306.appspot.com", fileName, fileType, file, new UploadObjectOptions()
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