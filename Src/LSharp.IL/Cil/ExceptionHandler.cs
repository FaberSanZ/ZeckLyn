// This code has been based from the sample repository "cecil": https://github.com/jbevain/cecil
// Copyright (c) 2020 - 2021 Faber Leonardo. All Rights Reserved. https://github.com/FaberSanZ
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)



namespace LSharp.IL.Cil
{

    public enum ExceptionHandlerType
    {
        Catch = 0,
        Filter = 1,
        Finally = 2,
        Fault = 4,
    }

    public sealed class ExceptionHandler
    {
        private Instruction try_start;
        private Instruction try_end;
        private Instruction filter_start;
        private Instruction handler_start;
        private Instruction handler_end;
        private TypeReference catch_type;
        private ExceptionHandlerType handler_type;

        public Instruction TryStart
        {
            get { return try_start; }
            set { try_start = value; }
        }

        public Instruction TryEnd
        {
            get { return try_end; }
            set { try_end = value; }
        }

        public Instruction FilterStart
        {
            get { return filter_start; }
            set { filter_start = value; }
        }

        public Instruction HandlerStart
        {
            get { return handler_start; }
            set { handler_start = value; }
        }

        public Instruction HandlerEnd
        {
            get { return handler_end; }
            set { handler_end = value; }
        }

        public TypeReference CatchType
        {
            get { return catch_type; }
            set { catch_type = value; }
        }

        public ExceptionHandlerType HandlerType
        {
            get { return handler_type; }
            set { handler_type = value; }
        }

        public ExceptionHandler(ExceptionHandlerType handlerType)
        {
            this.handler_type = handlerType;
        }
    }
}
