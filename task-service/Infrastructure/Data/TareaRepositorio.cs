using Dapper;
using System.Data;
using task_service.Domain.Entities;
using task_service.Domain.Interfaces;

namespace task_service.Infrastructure.Data
{
    public class TareaRepositorio : ITareaRepositorio{
        #region Atributos
        private readonly IDbConnection dbConnection;
        #endregion

        #region Constructores
        public TareaRepositorio(IDbConnection connection) {
            this.dbConnection = connection;
        }
        #endregion

        #region Métodos
        public async Task<Tarea?> GetByIdAsync(Guid id) {
            var sql = "Select * From tareas where id = @Id";
            return await dbConnection.QueryFirstOrDefaultAsync<Tarea>(sql, new { Id = id });
        }
        public async Task<Tarea?> GetByCodigoAsync(string codigo) {
            var sql = "Select * From tareas where codigotarea = @Codigo";
            return await dbConnection.QueryFirstOrDefaultAsync<Tarea?>(sql, new { Codigo = codigo });
        }
        public async Task<IEnumerable<Tarea>> GetAllAsync() {
            var sql = "Select * From tareas";
            return await dbConnection.QueryAsync<Tarea>(sql);
        }
        public async Task<Tarea> CreateAsync(Tarea tarea) {
            var sql = @"Insert into tareas (id, codigotarea, titulo, descripcion, criteriosaceptacion, fechainicio, fechafin, tiempodias, estadotarea, estado, usuarioid)
                           values (@Id, @CodigoTarea, @Titulo, @Descripcion, @CriteriosAceptacion,@FechaInicio, @FechaFin, @TiempoDias, @EstadoTarea, @Estado, @UsuarioId);";

            if (dbConnection.State == ConnectionState.Closed)
                dbConnection.Open();

            using var transaction = dbConnection.BeginTransaction();
            try {
                var affected = await dbConnection.ExecuteAsync(sql, tarea, transaction);
                if (affected == 0) {
                    transaction.Rollback();
                    throw new Exception($"No se pudo insertar la tarea con el Id : {tarea.Id}");
                }

                transaction.Commit();
                return tarea;
            }
            catch (Exception ex) {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateClientAsync(Guid id, Guid usuarioId) {
            var sql = "Update tareas set usuarioid = @UsuarioId where id = @Id";
            
            if (dbConnection.State == ConnectionState.Closed)
                dbConnection.Open();

            using var transaction = dbConnection.BeginTransaction();
            try {
                var affected = await dbConnection.ExecuteAsync(sql, new { UsuarioId = usuarioId, Id = id }, transaction);
                if (affected == 0) {
                    transaction.Rollback();
                    throw new Exception($"No se pudo actualizar la tarea con el Id : {id}");
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
