using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using System.Configuration;

namespace ClassLibrary.Transaction
{
    public class TransactionScope : IDisposable
    {
        private bool rootScope = false;

        private Dictionary<string, Transaction> transactionList = Transaction.CurrentTransactions;

        public bool Completed { get; private set; }

        public TransactionScope()
        {
            rootScope = !Transaction.TransactionStart;

            if (rootScope)
                Transaction.TransactionStart = true;
        }

        //public TransactionScope(IsolationLevel isolationLevel = IsolationLevel.Unspecified,
        //    Func<string, DbProviderFactory> getFactory = null)
        //{
        //    rootScope = !Transaction.TransactionStart;

        //    if (rootScope)
        //        Transaction.TransactionStart = true;
        //}

        public void Complete()
        {
            if (rootScope)
                this.Completed = true;
        }

        public void Dispose()
        {
            if (rootScope)
            {
                try
                {
                    Dictionary<string, Transaction> currentTransactions = Transaction.CurrentTransactions;
                    Transaction.CurrentTransactions = transactionList;
                  
                    if (currentTransactions != null)
                    {
                        if (!this.Completed)
                        {

                            foreach (Transaction tran in currentTransactions.Values)
                            {
                                tran.Rollback();
                            }
                        }

                        foreach (Transaction tran in currentTransactions.Values)
                        {
                            CommittableTransaction commitTran = tran as CommittableTransaction;

                            if (commitTran != null)
                            {
                                if (this.Completed)
                                {
                                    commitTran.Commit();
                                }

                                commitTran.Dispose();
                            }
                        }
                    }
                }
                finally
                {
                    rootScope = false;
                    Transaction.TransactionStart = false;
                }
            }
        }
    }
}