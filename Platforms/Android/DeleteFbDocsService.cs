using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Chess.Models;
using Plugin.CloudFirestore;
using Chess.ModelsLogic;

namespace Chess.Platforms.Android
{
    /// Background Android service for cleaning up old game documents from Firestore
    [Service]
    public class DeleteFbDocsService : Service
    {
        #region Fields
        private bool isRunning = true;
        private readonly FbData fbd = new();
        #endregion
        #region Public Methods
        /// Starts a background thread to execute the cleanup logic when the service is commanded
        public override StartCommandResult OnStartCommand(Intent? intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            ThreadStart threadStart = new(DeleteFbDocs);
            Thread thread = new(threadStart);
            thread.Start();
            return base.OnStartCommand(intent, flags, startId);
        }
        /// Bound service functionality is not implemented for this cleanup task
        public override IBinder? OnBind(Intent? intent)
        {
            return null;
        }
        /// Stops the background loop and cleans up service resources
        public override void OnDestroy()
        {
            isRunning = false;
            base.OnDestroy();
        }
        #endregion
        #region Private Methods
        /// Periodic loop that queries for and deletes documents older than 24 hours
        private void DeleteFbDocs()
        {
            while (isRunning)
            {
                // Fetch games created more than a day ago
                fbd.GetDocumentsWhereLessThan(Keys.GamesCollection, nameof(GameModel.Created), DateTime.Now.AddDays(-1), OnComplete);
                Thread.Sleep(Keys.OneHourInMilliseconds);
            }
            StopSelf();
        }
        /// Processes the query snapshot and deletes each expired document found
        private void OnComplete(IQuerySnapshot qs)
        {
            foreach (IDocumentSnapshot doc in qs.Documents)
                fbd.DeleteDocument(Keys.GamesCollection, doc.Id, (task) => { });
        }
        #endregion
    }
}