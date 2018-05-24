using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyFinance.Domain;
using MyFinance.Data.Infrastructure;
using MyFinance.Data;


namespace MyFinance.Service
{
    public interface ISecurityService
    {
        IEnumerable<User> GetUsers();
        User GetUser(string userName);
        void CreateUser(User user);
        void DeleteUser(string userName);
        
        IEnumerable<Role> Roles();
        Role GetRole(string roleName);
        void CreateRole(Role role);
        void DeleteRole(string roleName);
        void Save();

        void AssignRole(string userName, List<string> roleNames);
      

    }
    public class SecurityService : ISecurityService
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IUnitOfWork unitOfWork;
        public SecurityService(IUserRepository userRepository,IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.unitOfWork = unitOfWork;
        } 
        public IEnumerable<User> GetUsers()
        {
            var users = userRepository.GetAll();
            return users;
        }

        public User GetUser(string userName)
        {
            var user = userRepository.GetById(userName);
            return user;
        }

        public void CreateUser(User user)
        {
            userRepository.Add(user);
            Save();
        }

        public void DeleteUser(string userName)
        {
            var user = GetUser(userName);
            userRepository.Delete(user);
            Save();
        }

        public IEnumerable<Role> Roles()
        {
            var roles = roleRepository.GetAll();
            return roles;
        }

        public Role GetRole(string roleName)
        {
            var role = roleRepository.GetById(roleName);
            return role;
        }

        public void CreateRole(Role role)
        {
            roleRepository.Add(role);
            Save();
        }

        public void DeleteRole(string roleName)
        {
            var role = GetRole(roleName);
            roleRepository.Delete(role);
            Save();
        }
        public void AssignRole(string userName, List<string> roleNames)
        {
            userRepository.AssignRole(userName, roleNames);
            Save();
        }
        
        public void Save()
        {
            unitOfWork.Commit();
        }
      
    }
}
