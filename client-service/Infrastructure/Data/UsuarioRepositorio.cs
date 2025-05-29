using client_service.Domain.Entities;
using client_service.Domain.Exceptions;
using client_service.Domain.Interfaces;
using Dapper;
using System.Data;

namespace client_service.Infrastructure.Data {
    public class UsuarioRepositorio : IUsuarioRepositorio {
        #region Atributos
        private readonly IDbConnection dbConnection;
        #endregion

        #region Constructores
        public UsuarioRepositorio(IDbConnection connection) {
            this.dbConnection = connection;
        }
        #endregion

        #region Métodos
        public async Task<Usuario?> GetByIdAsync(Guid id) {
            var sql = "Select * From usuarios where id = @Id";
            return await dbConnection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Id = id });
        }
        public async Task<Usuario?> GetByIdentificationAsync(string identificacion) {
            var sql = "Select * From usuarios where identificacion = @Identificacion";
            return  await dbConnection.QueryFirstOrDefaultAsync<Usuario?>(sql, new { Identificacion = identificacion });
        }
        public async Task<Usuario?> GetByEmailAsync(string email) {
            var sql = "Select * From usuarios where email = @Email";
            return await dbConnection.QueryFirstOrDefaultAsync<Usuario?>(sql, new { Email = email });
        }
        public async Task<IEnumerable<Usuario>> GetAllAsync() {
            var sql = "Select * From usuarios";
            return await dbConnection.QueryAsync<Usuario>(sql);
        }
        public async Task<Usuario> CreateAsync(Usuario usuario) {
            var sql = @"Insert into usuarios (id, identificacion, nombres, apellidos, edad, cargo, email, estado) 
                        values (@Id, @Identificacion, @Nombres, @Apellidos, @Edad, @Cargo, @Email, @Estado)";

            if (dbConnection.State == ConnectionState.Closed)
                dbConnection.Open();

            using var transaction = dbConnection.BeginTransaction();
            try {
                var affected = await dbConnection.ExecuteAsync(sql, usuario, transaction);
                if (affected == 0) {
                    transaction.Rollback();
                    throw new Exception($"No se pudo insertar el usuario con el Id : {usuario.Id}");
                }

                transaction.Commit();
                return usuario;
            }
            catch (Exception ex) {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateAsync(Usuario usuario) {
            if (dbConnection.State == ConnectionState.Closed)
                dbConnection.Open(); 

            using var transaction = dbConnection.BeginTransaction();
            try {
                var sql = @"Update usuarios Set 
                        identificacion = @Identificacion,
                        nombres = @Nombres,
                        apellidos = @Apellidos,
                        edad = @Edad,
                        cargo = @Cargo,
                        email = @Email
                    Where id = @Id";

                var affected = await dbConnection.ExecuteAsync(sql, usuario, transaction);

                if (affected == 0) {
                    transaction.Rollback();
                    throw new Exception($"No se pudo actualizar el usuario con Id {usuario.Id}.");
                }

                transaction.Commit();
            }
            catch (Exception ex) {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
        public async Task DeleteAsync(Guid id) {
            if (dbConnection.State == ConnectionState.Closed)
                dbConnection.Open();

            using var transaction = dbConnection.BeginTransaction();
            try {
                var sql = "Delete From usuarios Where id = @Id";
                var affected = await dbConnection.ExecuteAsync(sql, new { Id = id }, transaction);

                if (affected == 0) {
                    transaction.Rollback();
                    throw new Exception($"No se pudo eliminar el usuario con Id : {id}.");
                }

                transaction.Commit();
            }
            catch (Exception ex) {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
