using MySql.Data.MySqlClient;
using System;
using WashinqV2.Models;

namespace WashinqV2.Helpers
{
    public static class LogActivity
    {
        /// <summary>
        /// Insert log activity ke database
        /// </summary>
        /// <param name="action">Jenis aksi (Tambah Data, Edit Data, Hapus Data, Login, dll)</param>
        /// <param name="description">Deskripsi detail aksi yang dilakukan</param>
        public static void Insert(string action, string description)
        {
            try
            {
                using (var conn = Database.Database.GetConnection())
                {
                    string query = @"INSERT INTO logs 
                                   (user_id, action, description, created_at, updated_at) 
                                   VALUES 
                                   (@userId, @action, @description, NOW(), NOW())";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", UserSession.id);
                        cmd.Parameters.AddWithValue("@action", action);
                        cmd.Parameters.AddWithValue("@description", description);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Silent fail - jangan ganggu proses utama kalau log gagal
                // Optional: log ke file atau console untuk debugging
                Console.WriteLine($"Failed to insert log: {ex.Message}");
            }
        }

        /// <summary>
        /// Insert log dengan info tambahan (opsional)
        /// </summary>
        public static void Insert(string action, string description, int targetId, string targetType)
        {
            string fullDescription = $"{description} | Target: {targetType} ID {targetId}";
            Insert(action, fullDescription);
        }
    }
}
