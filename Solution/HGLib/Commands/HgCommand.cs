﻿using System.Collections.Generic;

namespace HgLib
{
    public interface HgCommand
    {
        void Run(HgStatus status, List<string> dirtyFilesList);
    }
}