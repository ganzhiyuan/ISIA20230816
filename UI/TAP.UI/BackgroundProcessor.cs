using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Text;

namespace TAP.UI
{
    /// <summary>
    /// This runs background work.
    /// </summary>
    public class BackgroundProcessor:Form    
    {
        #region Delegate

        /// <summary>
        /// Function delegate
        /// </summary>
        public delegate void RunFunction();

        private delegate void SetUIDelegate();

        #endregion

        #region Fields

        /// <summary>
        /// Background worker
        /// </summary>
        public BackgroundWorker _backgroundWorker;

        /// <summary>
        /// Function
        /// </summary>
        public RunFunction _runFunction;

        private bool _isInitializing;
        private Form _processingForm;
        private bool _isCompleted;
        private readonly int _estimatedTime = -1;
        private long _total;

        #endregion

        /// <summary>
        /// This creates instnace of BackgroundProcessor
        /// </summary>
        /// <param name="function">Function for run</param>
        /// <param name="form">Form</param>
        /// <param name="total">Total progress</param>
        public BackgroundProcessor(RunFunction function, Form form, long total)
        {
            this.InitializeBackgroundProcessing(function, total);
        }

        /// <summary>
        /// This creates instnace of BackgroundProcessor
        /// </summary>
        /// <param name="function">Function for run</param>
        /// <param name="form">Form</param>
        /// <param name="total">Total progress</param>
        /// <param name="isInitializing">If true, this is initializeing background processor</param>
        public BackgroundProcessor(RunFunction function, Form form, long total, bool isInitializing)
        {
            _isInitializing = true;
            this.InitializeBackgroundProcessing(function, total);
        }

        #region Public Methods

        /// <summary>
        /// This method starts  background work
        /// </summary>
        public void Start()
        {
            #region Start

            _backgroundWorker.RunWorkerAsync();

            var tmpThread = new Thread(this.CheckCompletion);
            tmpThread.Start();

            if (_isInitializing)
            {
                _processingForm = new FormInitializing(this._total);
                _processingForm.StartPosition = FormStartPosition.CenterScreen;
                ((FormInitializing)_processingForm).AbortProcessing += OnAbortProcessinfForm;
            }
            else
            {
                _processingForm = new FormProcessing(this._total);
                ((FormProcessing)_processingForm).AbortProcessing += OnAbortProcessinfForm;
            }

            
            _processingForm.ShowDialog();

            #endregion
        }

        /// <summary>
        /// This method stops backgorund work.
        /// </summary>
        public void Stop()
        {
            #region Stop

            this.StopAndClose();

            #endregion
        }

        /// <summary>
        /// Disposer
        /// </summary>
        public new void Dispose()
        {
            #region Dispose

            _processingForm.Dispose();
            _backgroundWorker.Dispose();

            #endregion
        }

        #endregion

        #region Privte Methods

        private void CheckCompletion()
        {
            #region Check Completion

            while (true)
            {
                if (_isCompleted)
                    break;

                if (_backgroundWorker.CancellationPending)
                {
                    _backgroundWorker.CancelAsync();
                    break;
                }

                Thread.Sleep(100);

                if (_isCompleted)
                    break;

                if (!_backgroundWorker.CancellationPending && !_isCompleted)
                {
                    try
                    {
                        _backgroundWorker.ReportProgress(0);
                    }
                    catch
                    {
                    }
                }
                
            }

            #endregion
        }

        private void OnAbortProcessinfForm(Form form)
        {
            #region On Abort Processing Form

            this.StopAndClose();

            #endregion
        }

        private void StopAndClose()
        {
            #region Stop and Close

            if (!_backgroundWorker.CancellationPending)
                _backgroundWorker.CancelAsync();

            this.CloseProcessingForm();

            AsyncMessage.Progress = 0;
            AsyncMessage.Message = string.Empty;

            #endregion
        }

        private void CloseProcessingForm()
        {
            #region Close Processinf Form

            if (_processingForm != null)
                _processingForm.Dispose();

            #endregion
        }

        #endregion

        #region Event Handers

        private void InitializeBackgroundProcessing(RunFunction function, long total)
        {
            #region InitializeBackgroundProcessing

            AsyncMessage.Progress = 0;
            AsyncMessage.Message = string.Empty;

            _runFunction = function;
            _total = total;
            //FormSize = size;
            //FormLocation = location;

            _backgroundWorker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };

            _backgroundWorker.DoWork += BWDoWork;
            _backgroundWorker.RunWorkerCompleted += CompletedBWWorker;
            _backgroundWorker.ProgressChanged += BWOnProcessChanged;

            #endregion
        }

        private void DisplayMessage()
        {
            #region Display Message

            if (_processingForm != null)
            {
                if(_isInitializing)
                    ((FormInitializing)_processingForm).ViewProcessing();
                else
                    ((FormProcessing)_processingForm).ViewProcessing();
            }
            
            

            #endregion
        }

        private void BWOnProcessChanged(object sender, ProgressChangedEventArgs args)
        {
            #region BWOnProcessChanged

            if (_processingForm != null)
            {
                if (_isInitializing)
                    ((FormInitializing)_processingForm).ViewProcessing();
                else
                    ((FormProcessing)_processingForm).ViewProcessing();
            }

            #endregion
        }

        private void CompletedBWWorker(object sender, RunWorkerCompletedEventArgs args)
        {
            #region Complted BW Worker

            _isCompleted = true;
            CloseProcessingForm();

            #endregion
        }

        private void BWDoWork(object sender, DoWorkEventArgs args)
        {
            #region Completed BW Run

            if (_runFunction == null) return;

            var tmpProgressValue = 0;

            if (_estimatedTime != -1)
                tmpProgressValue = _estimatedTime;

            var tmpBackgroundWorker = sender as BackgroundWorker;

            //if (tmpBackgroundWorker != null)
            //    tmpBackgroundWorker.ReportProgress(tmpProgressValue);

            if (this.InvokeRequired)
            {
                this.Invoke(new SetUIDelegate(_runFunction));
            }
            else
            {
                _runFunction();
            }

            #endregion
        }

        #endregion
    }
}
