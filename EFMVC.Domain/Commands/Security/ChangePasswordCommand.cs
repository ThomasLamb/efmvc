﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFMVC.CommandProcessor.Command;

namespace EFMVC.Domain.Commands
{
    public class ChangePasswordCommand : ICommand
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
