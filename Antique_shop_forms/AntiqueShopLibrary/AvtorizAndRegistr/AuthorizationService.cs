using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiqueShopLibrary
{
    /// <summary>
    /// Класс управления правами доступа.
    /// Проверяет права пользователя при назначении ролей.
    /// </summary>
    public class AuthorizationService
    {
        /// <summary>
        /// Проверяет, является ли пользователь администратором.
        /// </summary>
        public bool IsAdmin(User user)
        {
            if (user == null)
            {
                return false;
            }

            if (user.Role == Role.Admin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Проверяет наличие права на назначение роли.
        /// Право есть только у администратора.
        /// </summary>
        public bool CanAssignRole(User user)
        {
            return IsAdmin(user);
        }
    }
}