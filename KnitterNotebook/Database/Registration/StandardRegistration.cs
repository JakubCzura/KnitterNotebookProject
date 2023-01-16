﻿using KnitterNotebook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Database.Registration
{
    public class StandardRegistration : IRegistration
    {
        public async Task<bool> RegisterUser(User user, KnitterNotebookContext knitterNotebookContext)
        {
            if (await IfUserAlreadyExists(user, knitterNotebookContext))
            {
                throw new Exception($"User with name { user.Nickname } already exists");
            }
            else
            {
                await knitterNotebookContext.AddAsync(user);
                await knitterNotebookContext.SaveChangesAsync();
                return true;
            }        
        }

        public static async Task<bool> IfUserAlreadyExists(User user, KnitterNotebookContext knitterNotebookContext)
        {
            if (await knitterNotebookContext.Users.AnyAsync(x => x.Nickname == user.Nickname || x.Email == user.Email))
            {
                return true;
            }
            return false;
        }
    }
}