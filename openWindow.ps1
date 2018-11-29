Param(
    [string]
    $Text
)

$npipeClient = new-object System.IO.Pipes.NamedPipeClientStream(".", 'advantage_pipe', [System.IO.Pipes.PipeDirection]::InOut,
                                                                [System.IO.Pipes.PipeOptions]::None, 
                                                                [System.Security.Principal.TokenImpersonationLevel]::Impersonation)

$npipeClient.Connect()
$pipeReader = new-object System.IO.StreamReader($npipeClient)
$pipeWriter = new-object System.IO.StreamWriter($npipeClient)
$pipeWriter.AutoFlush = $true

$textEscaped = $Text.Replace("\", "\\").Replace("`"", "\`"")
$pipeWriter.WriteLine("{`"command`": `"openWindow`", `"body`": {`"text`":`"$textEscaped`"}}")
$pipeWriter.Write("`0")

while ($true) {
    $buffer = [char[]]::new(10240)
    $i = 0

    $byteCount = $pipeReader.Read($buffer, $i, 1024)
    if ($byteCount -eq -1) {
        break
    }

    $i += $byteCount

    $endIndex = $buffer.IndexOf([char]"`0")
    if (($endIndex -ge 0) -and ($endIndex -le $i)) {
        if ($endIndex -gt 0) {
            $str = [string]::new($buffer, 0, $endIndex)

            $str
        }

        break
    }
}

$npipeClient.Dispose()