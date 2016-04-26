using System;

namespace ThumNet.EasyAD.Managers
{
    public static class ManagerFactory
    {
        private static Type _activeType = typeof(ActiveDirectoryManager);
        private static IGroupBasedUserManager _instance = null;   

        public static void SetManager<T>() where T : IGroupBasedUserManager
        {
            _activeType = typeof(T);
        }

        public static IGroupBasedUserManager GetManager()
        {
            if (_instance == null || _instance.GetType() != _activeType)
            {
                _instance = (IGroupBasedUserManager)Activator.CreateInstance(_activeType);
            }
            return _instance;
        }
    }
}
