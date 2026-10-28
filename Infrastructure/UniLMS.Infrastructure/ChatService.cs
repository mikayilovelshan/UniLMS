using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Chat;
using UniLMS.Persistence.Contexts;

namespace UniLMS.Infrastructure
{
    public class ChatService(AppDbContext _context) : IChatService
    {
        public Task<ChatResponseDTO> GetResponseAsync(ChatRequestDTO request, string userId)
        {
            var coursesCount = _context.Courses 
        }
    }
}
