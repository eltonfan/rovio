using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver.API
{
    /// <summary>
    /// Response Code.
    /// </summary>
    public enum ResponseCodes
    {
        /// <summary>
        /// CGI command successful
        /// </summary>
        SUCCESS = 0,
        /// <summary>
        /// CGI command general failure
        /// </summary>
        FAILURE = 1,
        /// <summary>
        /// Robot is executing autonomous function
        /// </summary>
        ROBOT_BUSY = 2,
        /// <summary>
        /// CGI command not implemented
        /// </summary>
        FEATURE_NOT_IMPLEMENTED = 3,
        /// <summary>
        /// CGI nav command: unknown action requested
        /// </summary>
        UNKNOWN_CGI_ACTION = 4,
        /// <summary>
        /// No NS signal available
        /// </summary>
        NO_NS_SIGNAL = 5,
        /// <summary>
        /// Path memory is full
        /// </summary>
        NO_EMPTY_PATH_AVAILABLE = 6,
        /// <summary>
        /// Failed to read FLASH
        /// </summary>
        FAILED_TO_READ_PATH = 7,
        /// <summary>
        /// FLASH error
        /// </summary>
        PATH_BASEADDRESS_NOT_INITIALIZED = 8,
        /// <summary>
        /// No path with such name
        /// </summary>
        PATH_NOT_FOUND = 9,
        /// <summary>
        /// Path name parameter is missing
        /// </summary>
        PATH_NAME_NOT_SPECIFIED = 10,
        /// <summary>
        /// Save path command received while not in recording mode
        /// </summary>
        NOT_RECORDING_PATH = 11,
        /// <summary>
        /// Flash subsystem failure
        /// </summary>
        FLASH_NOT_INITIALIZED = 12,
        /// <summary>
        /// Flash operation failed
        /// </summary>
        FAILED_TO_DELETE_PATH = 13,
        /// <summary>
        /// Flash operation failed
        /// </summary>
        FAILED_TO_READ_FROM_FLASH = 14,
        /// <summary>
        /// Flash operation failed
        /// </summary>
        FAILED_TO_WRITE_TO_FLASH = 15,
        /// <summary>
        /// Flash failed
        /// </summary>
        FLASH_NOT_READY = 16,
        /// <summary>
        /// NA
        /// </summary>
        NO_MEMORY_AVAILABLE = 17,
        /// <summary>
        /// NA
        /// </summary>
        NO_MCU_PORT_AVAILABLE = 18,
        /// <summary>
        /// NA
        /// </summary>
        NO_NS_PORT_AVAILABLE = 19,
        /// <summary>
        /// NA
        /// </summary>
        NS_PACKET_CHECKSUM_ERROR = 20,
        /// <summary>
        /// NA
        /// </summary>
        NS_UART_READ_ERROR = 21,
        /// <summary>
        /// One or more parameters are out of expected range
        /// </summary>
        PARAMETER_OUTOFRANGE = 22,
        /// <summary>
        /// One or more parameters are missing
        /// </summary>
        NO_PARAMETER = 23,
    }
}