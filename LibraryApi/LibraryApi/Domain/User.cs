using System.Collections.Generic;
using System.Net.Mail;
using LibraryApi.Enums;
using LibraryApi.Exceptions;
using LibraryApi.Infrastructure;
using SimpleCrypto;

namespace LibraryApi.Domain
{
    public class User
    {
        private User()
        {
        }

        public User(string firstName, string lastName, UserRole role, string email, string password)
        {
            Contract.Requires(IsEmailValid(email), ErrorCode.UserErrors.InvalidEmail);
            Contract.Requires(IsPasswordValid(password), ErrorCode.UserErrors.InvalidPassword);
            Contract.Requires(IsFirstNameValid(firstName), ErrorCode.UserErrors.InvalidFirstName);
            Contract.Requires(IsLastNameValid(lastName), ErrorCode.UserErrors.InvalidLastName);

            FirstName = firstName;
            LastName = lastName;
            Role = role;
            Email = email;

            var pbkdf2 = new PBKDF2();
            PasswordSalt = pbkdf2.GenerateSalt();
            PasswordHash = pbkdf2.Compute(password, PasswordSalt);
        }

        public int Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public UserRole Role { get; }
        public string Email { get; }
        public string PasswordHash { get; }
        public string PasswordSalt { get; }

        public List<Rental> Rentals { get; }

        private static bool IsPasswordValid(string password)
        {
            return !string.IsNullOrEmpty(password) && password.Length >= 8;
        }

        private static bool IsFirstNameValid(string firstName)
        {
            return !string.IsNullOrEmpty(firstName) && firstName.Length <= 100;
        }

        private static bool IsLastNameValid(string lastName)
        {
            return !string.IsNullOrEmpty(lastName) && lastName.Length <= 100;
        }


        private static bool IsEmailValid(string email)
        {
            if (string.IsNullOrEmpty(email) || email.Length > 100) return false;

            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public bool IsPasswordMatch(string password)
        {
            var pbkdf2 = new PBKDF2();
            var inputPasswordHash = pbkdf2.Compute(password, PasswordSalt);

            return PasswordHash == inputPasswordHash;
        }

        public void RentBook(Book book)
        {
            var rental = new Rental(Id, book.Id);
            Rentals.Add(rental);
        }
    }
}