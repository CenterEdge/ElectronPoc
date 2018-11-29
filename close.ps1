$npipeClient = new-object System.IO.Pipes.NamedPipeClientStream(".", 'advantage_pipe', [System.IO.Pipes.PipeDirection]::InOut,
                                                                [System.IO.Pipes.PipeOptions]::None, 
                                                                [System.Security.Principal.TokenImpersonationLevel]::Impersonation)

$npipeClient.Connect()
$pipeWriter = new-object System.IO.StreamWriter($npipeClient)
$pipeWriter.AutoFlush = $true

$pipeWriter.WriteLine("{`"command`": `"close`"}")
$pipeWriter.Write("`0")

$npipeClient.Dispose()