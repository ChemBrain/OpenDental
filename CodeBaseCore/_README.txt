﻿This project is a version of CodeBase that is compatible with .NET Core. It should only consist of file links to files in CodeBase. If you need
to add a link to a file from CodeBase, go ahead. If the file you want to add contains code that is not compatible with .NET Core, surround the
entire method with:
#if !DOT_NET_CORE"
{method}
#endif

DO NOT add new files to this project. If you want to add .NET Core code to CodeBaseCore, add the code to CodeBase and make a file link in
CodeBaseCore to that file.