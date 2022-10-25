﻿using Omnific.Model;

namespace Omnific.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class FantasyCharacterService : IFantasyCharacterService
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly OmnificContext _context;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="omnificContext"></param>
        public FantasyCharacterService(OmnificContext omnificContext)
        {
            _context = omnificContext;
        }
        /// <summary>
        /// public FantasyCharacter? CreateNewFantasyCharacter(string name, double height, double weight, string habitat, string description, string pictureURL, string ApiKeyInventor, string powers)
        /// Creates and returns a new fantasy character,
        /// null if is not possible create one
        /// </summary>
        /// <param name="name"></param>
        /// <param name="height"></param>
        /// <param name="weight"></param>
        /// <param name="habitat"></param>
        /// <param name="description"></param>
        /// <param name="pictureURL"></param>
        /// <param name="powers"></param>
        /// <returns></returns>
        public FantasyCharacter? CreateNewFantasyCharacter(string name, double height, double weight, string habitat, string description, string pictureURL, string powers)
        {
            var userLoggedIn = _context.Users.FirstOrDefault(user => user.Id == (_context.Logs.ToList().ElementAt(0).IdUser));
            // if the user is not logged in return null
            if (userLoggedIn == null) return null;
            string APIKeyInventor = userLoggedIn.ApiKey;
            var fantasyCharacter = new FantasyCharacter(name, height, weight, habitat, description, pictureURL, APIKeyInventor, powers);

            _context.Add(fantasyCharacter);
            _context.SaveChanges();
            //if the user is a viewer, set it as inventor
            if (userLoggedIn.UserType == UserType.Viewer) userLoggedIn.UserType = UserType.Inventor;

            return fantasyCharacter;
        }
        /// <summary>
        /// public FantasyCharacter? DeleteFantasyCharacterById(int id)
        /// delete and return a fantasy character by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FantasyCharacter? DeleteFantasyCharacterById(int id)
        {
            //retrieve the logged in user
            var userLoggedIn = _context.Users.FirstOrDefault(user => user.Id == (_context.Logs.ToList().ElementAt(0).IdUser));
            FantasyCharacter? fantasyCharacter = this.GetFantasyCharacterById(id);
            // if the user is not logged in return null
            if (userLoggedIn != null
                && fantasyCharacter != null
                && (userLoggedIn.IsUserAdministrator() || userLoggedIn.ApiKey == fantasyCharacter.APIKeyInventor))
            {
                _context.Remove(fantasyCharacter);
                _context.SaveChanges();
                return fantasyCharacter;
            }
            return null;
        }
        /// <summary>
        /// public List<FantasyCharacter> GetAllFantasyCharacters()
        /// returns the list of the fantasy character
        /// </summary>
        /// <returns></returns>
        public List<FantasyCharacter> GetAllFantasyCharacters()
        {
            var fantasyCharacters = _context.FantasyCharacters.ToList();
            return fantasyCharacters;
        }
        /// <summary>
        /// public List<FantasyCharacter> GetFantasyCharacterByAPIKeyInventor(string APIKeyInventor)
        /// return the fantasy character by APIKeyInventor,
        /// null if is not exists the APIKeyInventor
        /// </summary>
        /// <param name="APIKeyInventor"></param>
        /// <returns></returns>
        public List<FantasyCharacter> GetFantasyCharacterByAPIKeyInventor(string APIKeyInventor)
        {
            var fantasyCharacter = _context.FantasyCharacters.Where(f => f.APIKeyInventor == APIKeyInventor).ToList();
            return fantasyCharacter;
        }
        /// <summary>
        /// public FantasyCharacter? GetFantasyCharacterById(int id)
        /// returns the fantasy character by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FantasyCharacter? GetFantasyCharacterById(int id)
        {
            var fantasyCharacter = _context.FantasyCharacters.FirstOrDefault(f => f.Id == id);
            return fantasyCharacter;
        }
        /// <summary>
        /// public List<FantasyCharacter> GetFantasyCharacterByName(string name)
        /// returns the fantasy character by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<FantasyCharacter> GetFantasyCharacterByName(string name)
        {
            var fantasyCharacter = _context.FantasyCharacters.Where(fc => fc.Name == name).ToList();
                return fantasyCharacter;
        }
        /// <summary>
        /// public FantasyCharacter? UpdateFantasyCharacterById(int idFantasyCharacterToUpdate, string newName, double newHeight, double newWeight, string newHabitat, string newDescription, string newPictureURL, string newPowers)
        /// update the fantasy character found by id
        /// with new parameter passed.
        /// returns the fantasy character updated,
        /// null if is not possible update it.
        /// </summary>
        /// <param name="idFantasyCharacterToUpdate"></param>
        /// <param name="newName"></param>
        /// <param name="newHeight"></param>
        /// <param name="newWeight"></param>
        /// <param name="newHabitat"></param>
        /// <param name="newDescription"></param>
        /// <param name="newPictureURL"></param>
        /// <param name="newPowers"></param>
        /// <returns></returns>
        public FantasyCharacter? UpdateFantasyCharacterById(int idFantasyCharacterToUpdate,
            string newName, double newHeight, double newWeight, string newHabitat,
            string newDescription, string newPictureURL, string newPowers)
        {
            //retrieve the logged in user
            var userLoggedIn = _context.Users.FirstOrDefault(user => user.Id == (_context.Logs.ToList().ElementAt(0).IdUser));
            FantasyCharacter? fantasyCharacter = this.GetFantasyCharacterById(idFantasyCharacterToUpdate);
            // if the user is not logged in return null
            if (userLoggedIn != null
                && fantasyCharacter != null
                && (userLoggedIn.IsUserAdministrator() || userLoggedIn.ApiKey == fantasyCharacter.APIKeyInventor))
            {
                fantasyCharacter.Name = newName;
                fantasyCharacter.Height = newHeight;
                fantasyCharacter.Weight = newWeight;
                fantasyCharacter.Habitat = newHabitat;
                fantasyCharacter.Description = newDescription;
                fantasyCharacter.PictureURL = newPictureURL;
                fantasyCharacter.Powers = newPowers;
                _context.SaveChanges();
                return fantasyCharacter;
            }
            return null;
        }
    }
}
