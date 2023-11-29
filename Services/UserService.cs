using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Repositories;
using Services.Models;

namespace Services
{
    public class UserService : IDisposable
    {
        private readonly Model _db;

        /// <summary>
        /// 先使用內部建立 DbContext，重構後全部改為外部注入
        /// </summary>
        public UserService()
        {
            _db = new Model();
        }

        /// <summary>
        /// 外部注入資料內容，目前僅供測試，待重構後全部使用注入
        /// </summary>
        public UserService(Model dbContext)
        {
            _db = dbContext;
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public ServicesResultModel<List<UserModel>> GetUsers()
        {
            var result = new ServicesResultModel<List<UserModel>>();
            try
            {
                var models = _db.Users
                    .AsNoTracking()
                    .Select(user => new UserModel
                    {
                        Id = user.Id,
                        ConnectionId = user.ConnectionId,
                        Name = user.Name,
                        Email = user.Email,
                        Phone = user.Phone,
                        BuyerAverageStar = user.BuyerAverageStar,
                        SellerAverageStar = user.SellerAverageStar,
                        Permission = user.Permission,
                        Password = user.Password,
                        PasswordSalt = user.PasswordSalt,
                        Nickname = user.Nickname,
                        Picture = user.Picture,
                        Account = user.Account,
                        Birthday = user.Birthday
                    }).ToList();
                result.SetSuccess(models);
            }
            catch (Exception e)
            {
                result.SetException(e);
            }
            return result;
        }

        public ServicesResultModel<UserModel> GetUser(int id)
        {
            var result = new ServicesResultModel<UserModel>();
            try
            {
                var user = _db.Users
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id == id);
                if (user == default)
                {
                    result.SetError("找不到使用者", 1);
                }
                else
                {
                    var model = new UserModel
                    {
                        Id = user.Id,
                        ConnectionId = user.ConnectionId,
                        Name = user.Name,
                        Email = user.Email,
                        Phone = user.Phone,
                        BuyerAverageStar = user.BuyerAverageStar,
                        SellerAverageStar = user.SellerAverageStar,
                        Permission = user.Permission,
                        Password = user.Password,
                        PasswordSalt = user.PasswordSalt,
                        Nickname = user.Nickname,
                        Picture = user.Picture,
                        Account = user.Account,
                        Birthday = user.Birthday
                    };
                    result.SetSuccess(model);
                }
            }
            catch (Exception e)
            {
                result.SetException(e);
            }
            return result;
        }

        public ServicesResultModel<UserModel> GetUser(string account)
        {
            var result = new ServicesResultModel<UserModel>();
            try
            {
                var user = _db.Users
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Account == account);
                if (user == default)
                {
                    result.SetError("找不到使用者", 1);
                }
                else
                {
                    var model = new UserModel
                    {
                        Id = user.Id,
                        ConnectionId = user.ConnectionId,
                        Name = user.Name,
                        Email = user.Email,
                        Phone = user.Phone,
                        BuyerAverageStar = user.BuyerAverageStar,
                        SellerAverageStar = user.SellerAverageStar,
                        Permission = user.Permission,
                        Password = user.Password,
                        PasswordSalt = user.PasswordSalt,
                        Nickname = user.Nickname,
                        Picture = user.Picture,
                        Account = user.Account,
                        Birthday = user.Birthday
                    };
                    result.SetSuccess(model);
                }
            }
            catch (Exception e)
            {
                result.SetException(e);
            }
            return result;
        }

        public ServicesResultModel<UserModel> CreateUser(UserModel userModel)
        {
            var result = new ServicesResultModel<UserModel>();
            try
            {
                var user = _db.Users
                    .SingleOrDefault(x => x.Account == userModel.Account);
                if (user != default)
                {
                    result.SetError("帳號已存在", 1);
                }
                else
                {
                    var salt = Salt.CreateSalt();
                    user = new User
                    {
                        ConnectionId = userModel.ConnectionId,
                        Account = userModel.Account,
                        PasswordSalt = salt,
                        Password = Salt.GenerateHashWithSalt(userModel.Password, salt),
                        Nickname = userModel.Nickname,
                        Name = userModel.Name,
                        Picture = userModel.Picture,
                        Email = userModel.Email,
                        Phone = userModel.Phone,
                        BuyerAverageStar = userModel.BuyerAverageStar,
                        SellerAverageStar = userModel.SellerAverageStar,
                        Permission = 127,
                        Birthday = userModel.Birthday,
                    };
                    _db.Users.Add(user);
                    _db.SaveChanges();
                    result.SetSuccess(null);
                }
            }
            catch (Exception e)
            {
                result.SetException(e);
            }
            return result;
        }

        public ServicesResultModel<object> UpdateUser(UserModel userModel)
        {
            var result = new ServicesResultModel<object>();
            try
            {
                var user = _db.Users
                    .SingleOrDefault(x => x.Id == userModel.Id);
                if (user == default)
                {
                    result.SetError("找不到使用者", 1);
                }
                else
                {
                    user.PasswordSalt = Salt.CreateSalt();
                    user.Password = Salt.GenerateHashWithSalt(userModel.Password, user.PasswordSalt);
                    user.Nickname = string.IsNullOrEmpty(userModel.Nickname) ? user.Nickname : userModel.Nickname;
                    user.Name = string.IsNullOrEmpty(userModel.Name) ? user.Name : userModel.Name;
                    user.Picture = string.IsNullOrEmpty(userModel.Picture) ? user.Picture : userModel.Picture;
                    user.Email = string.IsNullOrEmpty(userModel.Email) ? user.Email : userModel.Email;
                    user.Phone = string.IsNullOrEmpty(userModel.Phone) ? user.Phone : userModel.Phone;
                    user.Birthday = userModel.Birthday ?? user.Birthday;
                    _db.SaveChanges();
                    result.SetSuccess(null);
                }
            }
            catch (Exception e)
            {
                result.SetException(e);
            }
            return result;
        }

        public ServicesResultModel<object> DeletePermission(int id)
        {
            var result = new ServicesResultModel<object>();
            try
            {
                var user = _db.Users
                    .SingleOrDefault(x => x.Id == id);
                if (user == default)
                {
                    result.SetError("找不到使用者", 1);
                }
                else
                {
                    user.Permission = 0;
                    _db.SaveChanges();
                    result.SetSuccess(null);
                }
            }
            catch (Exception e)
            {
                result.SetException(e);
            }
            return result;
        }
    }
}