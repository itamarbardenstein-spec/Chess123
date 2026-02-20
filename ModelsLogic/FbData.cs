using Chess.Models;
using Plugin.CloudFirestore;
using Plugin.FirebaseAuth;
namespace Chess.ModelsLogic
{
    public partial class FbData : FbDataModel
    {
        #region Properties
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
        public override string UserId
        {
            get => facl.User.Uid;
        }
        #endregion
        #region Public Methods
        public override async void CreateUserWithEmailAndPasswordAsync(string email, string password, string name, Action<System.Threading.Tasks.Task> OnComplete)
        {
            await facl.CreateUserWithEmailAndPasswordAsync(email, password, name).ContinueWith(OnComplete);
        }
        public override async void SignInWithEmailAndPasswordAsync(string email, string password, Action<System.Threading.Tasks.Task> OnComplete)
        {
            await facl.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(OnComplete);
        }
        public override string SetDocument(object obj, string collectonName, string id, Action<System.Threading.Tasks.Task> OnComplete)
        {
            IDocumentReference dr = string.IsNullOrEmpty(id) ? fs.Collection(collectonName).Document() : fs.Collection(collectonName).Document(id);
            dr.SetAsync(obj).ContinueWith(OnComplete);
            return dr.Id;
        }
        public override Plugin.CloudFirestore.IListenerRegistration AddSnapshotListener(string collectonName, Plugin.CloudFirestore.QuerySnapshotHandler OnChange)
        {
            ICollectionReference cr = fs.Collection(collectonName);
            return cr.AddSnapshotListener(OnChange);
        }
        public override Plugin.CloudFirestore.IListenerRegistration AddSnapshotListener(string collectonName, string id, Plugin.CloudFirestore.DocumentSnapshotHandler OnChange)
        {
            IDocumentReference cr = fs.Collection(collectonName).Document(id);
            return cr.AddSnapshotListener(OnChange);
        }
        public override async void GetDocumentsWhereEqualTo(string collectonName, string fName, object fValue, Action<IQuerySnapshot> OnComplete)
        {
            ICollectionReference cr = fs.Collection(collectonName);
            IQuerySnapshot qs = await cr.WhereEqualsTo(fName, fValue).GetAsync();
            OnComplete(qs);
        }
        public override async void GetDocumentsWhereLessThan(string collectonName, string fName, object fValue, Action<IQuerySnapshot> OnComplete)
        {
            ICollectionReference cr = fs.Collection(collectonName);
            IQuerySnapshot qs = await cr.WhereLessThan(fName, fValue).GetAsync();
            OnComplete(qs);
        }
        public override async void UpdateFields(string collectonName, string id, Dictionary<string, object> dict,Action<Task>OnComplete)
        {
            IDocumentReference dr = fs.Collection(collectonName).Document(id);
            await dr.UpdateAsync(dict).ContinueWith(OnComplete);
        }
        public override async void DeleteDocument(string collectonName, string id, Action<Task> OnComplete)
        {
            IDocumentReference dr = fs.Collection(collectonName).Document(id);
            await dr.DeleteAsync().ContinueWith(OnComplete);
        }
        public override async void ResetEmailPasswordAsync(string email, Action<Task> OnComplete)
        {
            await facl.ResetEmailPasswordAsync(email).ContinueWith(OnComplete);
        }
        public override async void SignInWithGoogleAsync(string idToken, Action<System.Threading.Tasks.Task> OnComplete)
        {
#if ANDROID
            try
            {
                GoogleAuthProviderWrapper googleProvider = new();
                IAuthCredential credential = googleProvider.GetCredential(idToken, null!);
                await CrossFirebaseAuth.Current.Instance.SignInWithCredentialAsync(credential)
                    .ContinueWith(OnComplete);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
#else
            // מניעת שגיאות בפלטפורמות שאינן אנדרואיד (iOS/Windows)
            await Task.CompletedTask;
#endif
        }
        #endregion
    }
}
