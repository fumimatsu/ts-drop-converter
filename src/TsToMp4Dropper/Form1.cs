using System.Collections.Concurrent;
using System.Diagnostics;
using System.Windows.Forms;

namespace TsToMp4Dropper;

public partial class Form1 : Form
{
    private readonly ConcurrentQueue<string> fileQueue = new();
    private readonly object queueLock = new();
    private string ffmpegPath = string.Empty;
    private bool isProcessing;

    public Form1()
    {
        InitializeComponent();
        AllowDrop = true;
        DragEnter += Form1_DragEnter;
        DragDrop += Form1_DragDrop;
        Load += Form1_Load;
    }

    private void Form1_Load(object? sender, EventArgs e)
    {
        ffmpegPath = Path.Combine(AppContext.BaseDirectory, "ffmpeg.exe");
        if (File.Exists(ffmpegPath))
        {
            AppendLog($"ffmpeg を検出しました: {ffmpegPath}");
            AppendLog("このウィンドウに .ts ファイルをドロップしてください。");
        }
        else
        {
            AppendLog("ffmpeg.exe が実行ファイルと同じフォルダに見つかりません。配置してから再試行してください。");
        }
    }

    private void Form1_DragEnter(object? sender, DragEventArgs e)
    {
        if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Any(f => string.Equals(Path.GetExtension(f), ".ts", StringComparison.OrdinalIgnoreCase)))
            {
                e.Effect = DragDropEffects.Copy;
                return;
            }
        }
        e.Effect = DragDropEffects.None;
    }

    private async void Form1_DragDrop(object? sender, DragEventArgs e)
    {
        if (!File.Exists(ffmpegPath))
        {
            AppendLog("ffmpeg.exe がありません。実行ファイルの横に配置してください。");
            return;
        }

        if (e.Data?.GetDataPresent(DataFormats.FileDrop) != true)
        {
            return;
        }

        var dropped = (string[])e.Data.GetData(DataFormats.FileDrop);
        var tsFiles = dropped
            .Where(f => string.Equals(Path.GetExtension(f), ".ts", StringComparison.OrdinalIgnoreCase))
            .Distinct()
            .ToList();

        if (tsFiles.Count == 0)
        {
            AppendLog(".ts ファイルが含まれていません。");
            return;
        }

        lock (queueLock)
        {
            foreach (var file in tsFiles)
            {
                fileQueue.Enqueue(file);
            }

            if (isProcessing)
            {
                AppendLog("キューに追加しました。実行中の処理が完了次第、順次変換します。");
                return;
            }

            isProcessing = true;
        }

        await ProcessQueueAsync();
    }

    private async Task ProcessQueueAsync()
    {
        while (true)
        {
            if (!fileQueue.TryDequeue(out var tsPath))
            {
                break;
            }

            await ProcessSingleAsync(tsPath);
        }

        lock (queueLock)
        {
            isProcessing = false;
        }
    }

    private async Task ProcessSingleAsync(string tsPath)
    {
        if (!File.Exists(tsPath))
        {
            AppendLog($"ファイルが見つかりません: {tsPath}");
            return;
        }

        var outputPath = Path.Combine(Path.GetDirectoryName(tsPath) ?? string.Empty,
            $"{Path.GetFileNameWithoutExtension(tsPath)}.mp4");

        AppendLog($"変換開始: {tsPath} → {outputPath}");

        var startInfo = new ProcessStartInfo
        {
            FileName = ffmpegPath,
            Arguments = $"-y -i \"{tsPath}\" -c:v copy -c:a copy \"{outputPath}\"",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        using var process = new Process
        {
            StartInfo = startInfo,
            EnableRaisingEvents = true
        };

        process.OutputDataReceived += (_, args) =>
        {
            if (!string.IsNullOrWhiteSpace(args.Data))
            {
                AppendLog(args.Data);
            }
        };

        process.ErrorDataReceived += (_, args) =>
        {
            if (!string.IsNullOrWhiteSpace(args.Data))
            {
                AppendLog(args.Data);
            }
        };

        try
        {
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            await process.WaitForExitAsync();

            if (process.ExitCode == 0)
            {
                AppendLog($"成功: {outputPath}");
            }
            else
            {
                AppendLog($"失敗 (ExitCode {process.ExitCode}): {tsPath}");
            }
        }
        catch (Exception ex)
        {
            AppendLog($"実行エラー: {ex.Message}");
        }
    }

    private void AppendLog(string message)
    {
        if (InvokeRequired)
        {
            BeginInvoke(new Action<string>(AppendLog), message);
            return;
        }

        var line = $"[{DateTime.Now:HH:mm:ss}] {message}";
        logTextBox.AppendText(line + Environment.NewLine);
    }
}
