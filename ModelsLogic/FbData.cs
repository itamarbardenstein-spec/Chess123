using Chess.Models;
using Plugin.CloudFirestore;
using Plugin.FirebaseAuth;

namespace Chess.ModelsLogic
{
    /// Concrete implementation of Firebase data services, including authentication and Firestore NoSQL operations
    public partial class FbData : FbDataModel
    {
        #region Properties
        /// Retrieves the authenticated user's display name from the Firebase Auth profile
        public override string DisplayName
        {
            get
            {
                string dn = string.Empty;
                if (facl.User != null)
                    dn = facl.User.Info.DisplayName;
                return dn;
            }
        }
        /// Retrieves the unique Firebase UID for the currently logged-in user
        public override string UserId
        {
            get => facl.User.Uid;
        }
        #endregion

        #region Public Methods
        /// Creates a new user record in Firebase Auth and triggers the completion callback
        public override async void CreateUserWithEmailAndPasswordAsync(string email, string password, string name, Action<System.Threading.Tasks.Task> OnComplete)
        {
            await facl.CreateUserWithEmailAndPasswordAsync(email, password, name).ContinueWith(OnComplete);
        }
        /// Authenticates an existing user using email/password and triggers the completion callback
        public override async void SignInWithEmailAndPasswordAsync(string email, string password, Action<System.Threading.Tasks.Task> OnComplete)
        {
            await facl.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(OnComplete);
        }
        /// Sets or merges object data into a Firestore document, creating a new ID if none is provided
        public override string SetDocument(object obj, string collectonName, string id, Action<System.Threading.Tasks.Task> OnComplete)
        {
            IDocumentReference dr = string.IsNullOrEmpty(id) ? fs.Collection(collectonName).Document() : fs.Collection(collectonName).Document(id);
            dr.SetAsync(obj).ContinueWith(OnComplete);
            return dr.Id;
        }
        /// Subscribes to real-time updates for an entire Firestore collection
        public override Plugin.CloudFirestore.IListenerRegistration AddSnapshotListener(string collectonName, Plugin.CloudFirestore.QuerySnapshotHandler OnChange)
        {
            ICollectionReference cr = fs.Collection(collectonName);
            return cr.AddSnapshotListener(OnChange);
        }
        /// Subscribes to real-time updates for a specific document within a collection
        public override Plugin.CloudFirestore.IListenerRegistration AddSnapshotListener(string collectonName, string id, Plugin.CloudFirestore.DocumentSnapshotHandler OnChange)
        {
            IDocumentReference cr = fs.Collection(collectonName).Document(id);
            return cr.AddSnapshotListener(OnChange);
        }
        /// Queries a collection for documents where a specific field matches the provided value
        public override async void GetDocumentsWhereEqualTo(string collectonName, string fName, object fValue, Action<IQuerySnapshot> OnComplete)
        {
            ICollectionReference cr = fs.Collection(collectonName);
            IQuerySnapshot qs = await cr.WhereEqualsTo(fName, fValue).GetAsync();
            OnComplete(qs);
        }
        /// Queries a collection for documents where a specific field value is less than the provided threshold
        public override async void GetDocumentsWhereLessThan(string collectonName, string fName, object fValue, Action<IQuerySnapshot> OnComplete)
        {
            ICollectionReference cr = fs.Collection(collectonName);
            IQuerySnapshot qs = await cr.WhereLessThan(fName, fValue).GetAsync();
            OnComplete(qs);
        }
        /// Updates specific fields within an existing Firestore document without overwriting the entire object
        public override async void UpdateFields(string collectonName, string id, Dictionary<string, object> dict, Action<Task> OnComplete)
        {
            IDocumentReference dr = fs.Collection(collectonName).Document(id);
            await dr.UpdateAsync(dict).ContinueWith(OnComplete);
        }
        /// Permanently removes a document from a Firestore collection by its ID
        public override async void DeleteDocument(string collectonName, string id, Action<Task> OnComplete)
        {
            IDocumentReference dr = fs.Collection(collectonName).Document(id);
            await dr.DeleteAsync().ContinueWith(OnComplete);
        }
        /// Triggers a password recovery email via Firebase Auth for the specified account
        public override async void ResetEmailPasswordAsync(string email, Action<Task> OnComplete)
        {
            await facl.ResetEmailPasswordAsync(email).ContinueWith(OnComplete);
        }
        /// Handshakes with Google Play Services to exchange an ID Token for a Firebase Credential, enabling seamless SSO
        /// This method bridges external Google Identity Providers with the internal Firebase User system.
        /// It creates a secure credential object, attempts the sign-in, and persists the user session.
        public override async void SignInWithGoogleAsync(string idToken, Action<System.Threading.Tasks.Task> OnComplete)
        {
#if ANDROID
            try
            {
                /// Initialize the specialized provider for Google-specific authentication logic
                GoogleAuthProviderWrapper googleProvider = new();
                /// Exchange the raw OAuth2 ID Token from the client for a standard Firebase Auth credential
                IAuthCredential credential = googleProvider.GetCredential(idToken, null!);
                /// Authenticate into the current Firebase instance using the generated Google credential
                /// Upon success, this integrates the Google profile data (email, name, photo) into the Firebase User object
                await CrossFirebaseAuth.Current.Instance.SignInWithCredentialAsync(credential)
                    .ContinueWith(OnComplete);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
#else
            await Task.CompletedTask;
#endif
        }
        #endregion
    }
}