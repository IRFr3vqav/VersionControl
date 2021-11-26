using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnitTestExample.Abstractions;
using UnitTestExample.Entities;
using UnitTestExample.Services;

namespace UnitTestExample.Controllers
{
    public class AccountController
    {        
        public IAccountManager AccountManager { get; set; }

        public AccountController()
        {
            AccountManager = new AccountManager();
        }

        public Account Register(string email, string password)
        {
            if(!ValidateEmail(email))
                throw new ValidationException(
                    "A megadott e-mail cím nem megfelelő!");
            if(!ValidatePassword(password))
                throw new ValidationException(
                    "A megadott jelszó nem megfelelő!\n" +
                    "A jelszó legalább 8 karakter hosszú kell legyen, csak az angol ABC betűiből és számokból állhat, és tartalmaznia kell legalább egy kisbetűt, egy nagybetűt és egy számot.");

            var account = new Account()
            {
                ID = Guid.NewGuid(),
                Email = email,
                Password = password
            };

            var newAccount = AccountManager.CreateAccount(account);

            return newAccount;
        }

        public bool ValidateEmail(string email)
        {            
            return Regex.IsMatch(
                email, 
                @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
        }

        public bool ValidatePassword(string password)
        {
            var van_kisbetu = new Regex(@"[a-z]+");
            var van_nagybetu = new Regex(@"[A-Z]+");
            var van_szam = new Regex(@"[0-9]+");
            var nyolc_karakter_hosszu = new Regex(@".{8,}");
            return van_kisbetu.IsMatch(password) && van_nagybetu.IsMatch(password) && van_szam.IsMatch(password) && nyolc_karakter_hosszu.IsMatch(password);
        }
    }
}
