using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Logging
{
    
    #region Enumerations
    #endregion


    /// <summary>
    /// Creates and writes to a log file.
    /// </summary>
    class Logger : IDisposable 
    {
        #region Revision History.

        ///////////////////////////////////////////////////////////
        // Write message to a log file.
        //
        //  Version Dated       Author      Remarks
        //  ------------------------------------------------------
        //  1.0.0   23/07/11    Chee Fai    Initial design.
        //
        ///////////////////////////////////////////////////////////


        #endregion


        #region Constants.
        #endregion


        #region Local Variables.

        private bool isDisposed = false;    // Flag object disposed.

        private string mLogPath;		    // Log path.
	    private string mLogName;		    // Log name.
	    
        #endregion


        #region Global Variables.

        #endregion


        #region Constructors.


        /// <summary>
        /// Default constructor.
        /// Last update ver: 1.0.0
        /// </summary>
        public Logger()
        {
        }
        

        /// <summary>
        /// Constructor overload.
        /// Last update ver: 1.0.0
        /// </summary>
        public Logger(string path, string logName)
        {
            LogPath = path;              
            LogName  = logName;
        }

        #endregion


        #region Properties.


        /// <summary>
        /// Get/Set log directory path.
        /// Last update ver: 1.0.0
        /// </summary>
        /// <exception cref="System.ArgumentException"></exception>
        public string LogPath
        {
            get
            {
                return mLogPath;    // Return log filename.	
            }
            set
            {  
                if ((value == null) || (value == String.Empty) || (!(Directory.Exists(value))) )
                {
                    throw new ArgumentException("Invalid path parameter.");
                }

                if (!(value.EndsWith("\\")))	// Without extension.
                {
                    value = String.Concat(value, "\\");
                }
                
                mLogPath = value;
            }
        }


        /// <summary>
        /// Get/Set log filename.
        /// Last update ver: 1.0.0
        /// </summary>
        /// <exception cref="System.ArgumentException"></exception>
        public string LogName
        {
            get
            {
                return mLogName;    // Return log filename.	
            }
            set
            {
                if  ( (value == null) || (value == String.Empty) )
                {
                    throw new ArgumentException("Filename parameter must not be null.");
                }
                
                if ( !(value.Contains(".")) )	// Without extension.
	            {
		            mLogName = String.Concat(value,".log"); 
	            }
                else
                {
                    mLogName = value;
                }
            }
        }
        

        #endregion


        #region Private Methods.
        #endregion


        #region Public Methods.


        /// <summary>
        /// Returns true if append message to a log successfully,
        /// else default to false.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="appendDate"></param>
        /// <returns></returns>
        public bool Append(string message, bool appendDate)
        {
            bool ret = false;
            string errMsg = String.Empty;
            StreamWriter sw = null;
            
            try
            {
                // Ensure object not disposed.
                if (isDisposed)
                {
                    throw new ObjectDisposedException("Logger");
                }
                
                // Validate members.
                if ( (!(Directory.Exists(mLogPath)) ) || 
                    (mLogName == "") || (mLogName == null) )	
	            {
		            throw new ArgumentException("Invalid log path or filename.");
	            }
            
                string path = String.Concat(mLogPath, mLogName); 
	            sw = File.AppendText(path);

                if (appendDate)
                {
	                // Get current date.
                    DateTime dtNow = DateTime.Now;
	                int intMonth = dtNow.Month;
	                int intDay = dtNow.Day;  
	
	                string strMonth = Convert.ToString(dtNow.Month);
	                string strDay = Convert.ToString(dtNow.Day);
	
	                if (intMonth < 10)
	                {
		                strMonth = String.Concat("0", intMonth); 
	                }

	                if (intDay < 10)
	                {
		                strDay = String.Concat("0", intDay); 
	                }

	                string today = String.Concat(dtNow.Year, "-", strMonth, "-", strDay, " ", dtNow.ToShortTimeString());   
	                message = String.Concat(today, "; ", message);
                }
                
                sw.WriteLine(message);
                ret = true;
	        }
            catch (Exception ex)
            {
                /////////////////////////////////////////////////////
                // Do not throw exception here, 
                // otherwise will lead to infinite call looping.
                /////////////////////////////////////////////////////

                errMsg = ex.Message;
            }
            finally
	        {
                if (sw != null)
                {
                    sw.Dispose();
                }
	        }

            return ret;
        }


        /// <summary>
        /// Returns true if all data have been successfully written in file,
        /// else default is false.
        /// </summary>
        /// <param name="createText"></param>
        /// <returns></returns>
        public bool IsWriteAllLines(string[] data)
        {
            bool ret = false;
            string errMsg = String.Empty;

            try
            {
                // Ensure object not disposed.
                if (isDisposed)
                {
                    throw new ObjectDisposedException("Logger");
                }

                 // Validate members.
                if ( (!(Directory.Exists(mLogPath)) ) || 
                    (mLogName == "") || (mLogName == null) )	
	            {
		            throw new ArgumentException("Invalid log path or filename.");
	            }
                
                string path = String.Concat(mLogPath, mLogName); 
	            File.WriteAllLines(path, data);
                ret = true;
            }
            catch (Exception ex)
            {
                /////////////////////////////////////////////////////
                // Do not throw exception here, 
                // otherwise will lead to infinite call looping.
                /////////////////////////////////////////////////////

                errMsg = ex.Message;
            }
            
            return ret;
        }

        #endregion


        #region Implement IDisposable.

        /// <summary>
        /// Last updated version: 1.0.0
        /// </summary>
        public void Dispose()
        {
            Dispose(true);              // Call the override method.
            GC.SuppressFinalize(this);  // Tell Garbage Collector not to call finalize method.
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!(isDisposed))
            {
                if (disposing)
                {
                    // Cleanup managed objects by calling their Dispose() methods.
                    
                }
                
                // Cleanup unmanaged objects.
            }
            isDisposed = true;
        }

        ~Logger()
        {
            Dispose(false);
        }

        #endregion



    }
}
