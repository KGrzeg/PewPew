using System;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Storage;

namespace PewPew {
    [Serializable]
    public class SaveGameDataSettings {
        public string Nick;
        public Color PlayerColor;
        public float Vol_Sfx,
                     Vol_Music;
        public static SaveGameDataSettings CreateFromSettings(){
            return new SaveGameDataSettings() {
                Nick = Settings.Nick,
                PlayerColor = Settings.Color_Player,
                Vol_Sfx = Settings.Volume_Sfx,
                Vol_Music = Settings.Volume_Music
            };
        }
    }
    [Serializable]
    public class SaveGameDataShip {
        public int Experience,
                   LevelSpeed,
                   LevelAgility,
                   LevelHealth;
        public SaveGameDataShip(Ship szip) {
            this.Experience = szip.Experience;
            this.LevelSpeed = szip.LevelSpeed;
            this.LevelAgility = szip.LevelAgility;
            this.LevelHealth = szip.LevelHealth;
        }
        public SaveGameDataShip() { }
    }
    [Serializable]
    public class SaveGameDataUser {
        public int Money,
                   Experience,
                   Ship;
        public SaveGameDataShip[] Ships;
        public bool[] BuyedShips;

        public static SaveGameDataUser CreateFromStatic() {
            SaveGameDataShip[] Szips = new SaveGameDataShip[User.Ships.Length];
            int i = 0;
            foreach( Ship ship in User.Ships )
                Szips[i++] = new SaveGameDataShip( ship );

            return new SaveGameDataUser() {
                Money = User.Money,
                Experience = User.Experience,
                Ship = User.Ship,
                Ships = Szips,
                BuyedShips = User.BuyedShips
            };
        }
    }
    public class StorageManager{
        private string SaveFileNameSettings = "settings.sav",
                       SaveFileNameUser = "account.sav",
                       StorageName = "Storage";
        //private StorageDevice device = null;
        private bool GameSaveRequestedSettings = false,
                     GameLoadRequestedSettings = false,
                     GameSaveRequestedUser = false,
                     GameLoadRequestedUser = false;
        private SaveGameDataSettings ObjectToSaveSettings = default( SaveGameDataSettings ),
                                     ObjectToLoadSettings = default( SaveGameDataSettings );
        private SaveGameDataUser ObjectToSaveUser = default( SaveGameDataUser ),
                                ObjectToLoadUser = default( SaveGameDataUser );
        public StorageManager() {
            //if( ( !Guide.IsVisible ) ) {
            //    IAsyncResult resultInit = StorageDevice.BeginShowSelector(
            //            PlayerIndex.One, null, null );
            //    if( resultInit.IsCompleted )
            //        this.device = StorageDevice.EndShowSelector( resultInit );
            //}
        }

        private void GetDevice(IAsyncResult result) {
            //device = StorageDevice.EndShowSelector( result );
            //if( device != null && device.IsConnected ) {
            //    if( this.GameSaveRequestedSettings == true ) {
            //        DoSaveGameSettings( device );
            //        this.GameSaveRequestedSettings = false;
            //    }
            //    if( this.GameLoadRequestedSettings == true ) {
            //        DoLoadGameSettings( device );
            //        this.GameLoadRequestedSettings = false;
            //    }
            //    if( this.GameSaveRequestedUser == true ) {
            //        DoSaveGameUser( device );
            //        this.GameSaveRequestedUser = false;
            //    }
            //    if( this.GameLoadRequestedUser == true ) {
            //        DoLoadGameUser( device );
            //        this.GameLoadRequestedUser = false;
            //    }
            //}
        }
        public void Update() {
            // If a save is pending, save as soon as the
            // storage device is chosen
            //if( ( this.GameLoadRequestedSettings ) ) {
            //    if( this.device != null && this.device.IsConnected ) {
            //        this.DoLoadGameSettings( this.device );
            //    }
            //    // Reset the request flag
            //    this.GameLoadRequestedSettings = false;
            //}
            //if( ( this.GameSaveRequestedSettings ) ) {
            //    if( this.device != null && this.device.IsConnected ) {
            //        this.DoSaveGameSettings( this.device );
            //    }
            //    // Reset the request flag
            //    this.GameSaveRequestedSettings = false;
            //}
        }
        public void SaveSettings(SaveGameDataSettings data) {
            // Set the request flag
            //if( ( !Guide.IsVisible ) && ( this.GameSaveRequestedSettings == false ) ) {
            //    this.ObjectToSaveSettings = data;
            //    this.GameSaveRequestedSettings = true;
            //    // Reset the device
            //    this.device = null;
            //    StorageDevice.BeginShowSelector(
            //            PlayerIndex.One, this.GetDevice, null );
            //}
        }
        public void LoadSettings() {
            // Set the request flag
            //if( ( !Guide.IsVisible ) && ( this.GameLoadRequestedSettings == false ) ) {
            //    this.GameLoadRequestedSettings = true;
            //    this.device = null;
            //    StorageDevice.BeginShowSelector(
            //            PlayerIndex.One, this.GetDevice, null );
            //}
        }
        public void SaveUser(SaveGameDataUser data) {
            // Set the request flag
            //if( ( !Guide.IsVisible ) && ( this.GameSaveRequestedUser == false ) ) {
            //    this.ObjectToSaveUser = data;
            //    this.GameSaveRequestedUser = true;
            //    // Reset the device
            //    this.device = null;
            //    StorageDevice.BeginShowSelector(
            //            PlayerIndex.One, this.GetDevice, null );
            //}
        }
        public void LoadUser() {
            //if( ( !Guide.IsVisible ) && ( this.GameLoadRequestedUser == false ) ) {
            //    this.GameLoadRequestedUser = true;
            //    this.device = null;
            //    StorageDevice.BeginShowSelector(
            //            PlayerIndex.One, this.GetDevice, null );
            //}
        }
        public bool SaveFileExist(string fileName) {
            //if( this.device.IsConnected == false )
            //    return false;

            //IAsyncResult result = this.device.BeginOpenContainer( this.StorageName, null, null );

            ////czekamy na WaitHandle
            //result.AsyncWaitHandle.WaitOne();

            //StorageContainer container = device.EndOpenContainer( result );

            ////zamykamy uchwyt
            //result.AsyncWaitHandle.Close();

            //if( container.FileExists( fileName ) == true )
            //    return true;
            //else
            //    return false;
            return true;
        }

        private void SetSettings() {
            if( this.ObjectToLoadSettings != null)
                Settings.Load( this.ObjectToLoadSettings );
        }
        private void SetUser() {
            if( this.ObjectToLoadUser != null )
                User.Update( this.ObjectToLoadUser );
        }

        /// <summary>
        /// This method serializes a data object into
        /// the StorageContainer for this game.
        /// </summary>
        /// <param name="device"></param>
        //private void DoSaveGameSettings(StorageDevice device) {
        //    //urwalo!
        //    if( device.IsConnected == false )
        //        return;

        //    // Open a storage container.
        //    IAsyncResult result =
        //        device.BeginOpenContainer( this.StorageName, null, null );

        //    // Wait for the WaitHandle to become signaled.
        //    result.AsyncWaitHandle.WaitOne();

        //    StorageContainer container = this.device.EndOpenContainer( result );

        //    // Close the wait handle.
        //    result.AsyncWaitHandle.Close();

        //    // Check to see whether the save exists.
        //    if( container.FileExists( this.SaveFileNameSettings ) )
        //        // Delete it so that we can create one fresh.
        //        container.DeleteFile( this.SaveFileNameSettings );

        //    // Create the file.
        //    Stream stream = container.CreateFile( this.SaveFileNameSettings );

        //    // Convert the object to XML data and put it in the stream.
        //    XmlSerializer serializer = new XmlSerializer( typeof( SaveGameDataSettings ) );
        //    serializer.Serialize( stream, this.ObjectToSaveSettings );

        //    //wyczysc obiekt
        //    this.ObjectToSaveSettings = new SaveGameDataSettings();

        //    // Close the file.
        //    stream.Close();

        //    // Dispose the container, to commit changes.
        //    container.Dispose();
        //}
        //private void DoSaveGameUser(StorageDevice device) {
        //    IAsyncResult result =
        //        device.BeginOpenContainer( this.StorageName, null, null );
        //    result.AsyncWaitHandle.WaitOne();
        //    StorageContainer container = this.device.EndOpenContainer( result );
        //    result.AsyncWaitHandle.Close();

        //    if( container.FileExists( this.SaveFileNameUser ) )
        //        container.DeleteFile( this.SaveFileNameUser );

        //    Stream stream = container.CreateFile( this.SaveFileNameUser );

        //    XmlSerializer serializer = new XmlSerializer( typeof( SaveGameDataUser ) );
        //    serializer.Serialize( stream, this.ObjectToSaveUser );

        //    this.ObjectToSaveUser = new SaveGameDataUser();
        //    stream.Close();
        //    container.Dispose();
        //}
        /// <summary>
        /// This method loads a serialized data object
        /// from the StorageContainer for this game.
        /// </summary>
        /// <param name="device"></param>
        //private void DoLoadGameSettings(StorageDevice device) {
        //    IAsyncResult result =
        //        device.BeginOpenContainer( this.StorageName, null, null );
        //    result.AsyncWaitHandle.WaitOne();
        //    StorageContainer container = device.EndOpenContainer( result );
        //    result.AsyncWaitHandle.Close();

        //    if( !container.FileExists( this.SaveFileNameSettings ) ) {
        //        container.Dispose();
        //        this.ObjectToLoadSettings = null;
        //        return;
        //    }
        //    Stream stream = container.OpenFile( this.SaveFileNameSettings, FileMode.Open );

        //    XmlSerializer serializer = new XmlSerializer( typeof( SaveGameDataSettings ) );
        //    SaveGameDataSettings data = ( SaveGameDataSettings ) serializer.Deserialize( stream );

        //    stream.Close();
        //    container.Dispose();
        //    this.ObjectToLoadSettings = data;
        //    this.SetSettings();
        //}
        //private void DoLoadGameUser(StorageDevice device) {
        //    IAsyncResult result =
        //        device.BeginOpenContainer( this.StorageName, null, null );
        //    result.AsyncWaitHandle.WaitOne();
        //    StorageContainer container = device.EndOpenContainer( result );
        //    result.AsyncWaitHandle.Close();

        //    if( !container.FileExists( this.SaveFileNameUser ) ) {
        //        container.Dispose();
        //        this.ObjectToLoadUser = null;
        //        return;
        //    }
        //    Stream stream = container.OpenFile( this.SaveFileNameUser, FileMode.Open );

        //    XmlSerializer serializer = new XmlSerializer( typeof( SaveGameDataUser ) );
        //    SaveGameDataUser data = ( SaveGameDataUser ) serializer.Deserialize( stream );

        //    stream.Close();
        //    container.Dispose();
        //    this.ObjectToLoadUser = data;
        //    this.SetUser();
        //}
    }
}
