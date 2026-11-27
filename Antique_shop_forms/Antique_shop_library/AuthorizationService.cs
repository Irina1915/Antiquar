using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antique_shop_library
{
    /// <summary>
    /// Класс управления правами доступа.
    /// Отвечает за проверку прав текущего пользователя при выполнении операций,
    /// связанных с назначением ролей.
    /// </summary>
    public class AuthorizationService
    {
        /// <summary>
        /// Проверяет, является ли указанный пользователь администратором.
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