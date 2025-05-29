﻿namespace task_service.Domain.Exceptions {
    public class ValidationException : Exception {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException(string message, IDictionary<string, string[]> errors) : base(message) {
            Errors = errors;
        }

        public ValidationException(string message) : base(message) {
            Errors = new Dictionary<string, string[]>();
        }
    }
}