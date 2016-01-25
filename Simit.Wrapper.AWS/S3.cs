namespace Simit.Wrapper.AWS
{
    #region Using Directive

    using Amazon;
    using Amazon.S3;
    using Amazon.S3.Transfer;
    using System;
    using System.IO;

    #endregion Using Directive

    public class S3
    {
        #region Public Methods

        public void PutObject(string bucketName, string key, string fileName, Stream file, RegionEndpoint endpoint)
        {
            if (bucketName == null) throw new ArgumentNullException("bucketName");
            if (key == null) throw new ArgumentNullException("key");
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (file == null) throw new ArgumentNullException("file");

            string clientAccessKey = System.Configuration.ConfigurationManager.AppSettings["aws:s3:accesskey"];
            string clientSecretKey = System.Configuration.ConfigurationManager.AppSettings["aws:s3:secretkey"];

            TransferUtility utility = new TransferUtility(clientAccessKey, clientSecretKey, endpoint);

            utility.Upload(new TransferUtilityUploadRequest
            {
                BucketName = bucketName,
                Key = key + "/" + fileName,
                InputStream = file,
                CannedACL = S3CannedACL.PublicRead
            });
        }

        public void DownloadObject(string bucketName, string key, string fileName, string saveFilePath)
        {
            if (bucketName == null) throw new ArgumentNullException("bucketName");
            if (key == null) throw new ArgumentNullException("key");
            if (fileName == null) throw new ArgumentNullException("fileName");

            string clientAccessKey = System.Configuration.ConfigurationManager.AppSettings["aws:s3:accesskey"];
            string clientSecretKey = System.Configuration.ConfigurationManager.AppSettings["aws:s3:secretkey"];

            TransferUtility utility = new TransferUtility(clientAccessKey, clientSecretKey, RegionEndpoint.EUWest1);

            TransferUtilityDownloadRequest transferUtilityDownloadRequest = new TransferUtilityDownloadRequest();
            transferUtilityDownloadRequest.BucketName = bucketName;
            transferUtilityDownloadRequest.Key = key + "/" + fileName;
            transferUtilityDownloadRequest.FilePath = saveFilePath;

            utility.Download(transferUtilityDownloadRequest);
        }

        #endregion Public Methods
    }
}