﻿using RestSharp;
using System;
using System.Windows.Forms;

namespace VerifyFreeRest_NET
{

    /// <summary>
    /// Esempio di utilizzo del servizio WS VERIFY Free per la verifica e la normalizzazione degli indirizzi italiani 
    /// realizzato da StreetMaster Italia
    /// 
    /// L'end point del servizio è 
    ///     https://streetmaster.streetmaster.it/smrest/webresources/verify_free
    ///     
    /// Per l'utilizzo registrarsi sul sito http://streetmaster.it e richiedere la chiave per il servizio Verify Free solo localita' 
    /// Il protocollo di comunicazione e' in formato JSON
    /// Per le comunicazioni REST è utilizzata la libreria opensource RestSharp (http://restsharp.org/)
    /// 
    ///  2016 - Software by StreetMaster (c)
    /// </summary>
    public partial class frmVerifyFreeRest : Form
    {
        int currCand = 0;

        VerifyFreeResponse outVerifyFree;


        public frmVerifyFreeRest()
        {
            InitializeComponent();
        }

        private void btnCallVerifyFree_Click(object sender, EventArgs e)
        {

            if (txtKey.Text==String.Empty)
            {
                MessageBox.Show("E' necessario specificare una chiave valida per il servizio VERIFY Free");
                txtKey.Focus();
                return;
            }

            Cursor = Cursors.WaitCursor;
            Application.DoEvents();


            // inizializzazione client del servizio VERIFY Free
            var clientVerifyFree = new RestClient
            {
                BaseUrl = new Uri("https://streetmaster.streetmaster.it")
            };

            var request = new RestRequest("smrest/webresources/verify_free", Method.GET)
            {
                RequestFormat = DataFormat.Json
            };


            // valorizzazione input
            // per l'esempio viene valorizzato un insieme minimo dei parametri
            request.AddParameter("Key", txtKey.Text);
            request.AddParameter("Localita", txtInComune.Text);
            request.AddParameter("Cap", txtInCap.Text);
            request.AddParameter("Provincia", txtInProvincia.Text);
            request.AddParameter("Localita2", String.Empty);

            var response = clientVerifyFree.Execute<VerifyFreeResponse>(request);
            outVerifyFree = response.Data;


            //  output generale
            txtOutEsito.Text = outVerifyFree.Norm.ToString();
            txtOutCodErr.Text = outVerifyFree.CodErr.ToString();
            txtOutNumCand.Text = outVerifyFree.NumCand.ToString();

            currCand = 0;

            // dettaglio del primo candidato se esiste
            // nella form di output e' riportato solo un sottoinsieme di tutti i valori restituiti
            if (outVerifyFree.Output.Count > 0)
            {
                txtOutCap.Text = outVerifyFree.Output[currCand].Cap;
                txtOutComune.Text = outVerifyFree.Output[currCand].Comune;
                txtOutFrazione.Text = outVerifyFree.Output[currCand].Frazione;
                txtOutProvincia.Text = outVerifyFree.Output[currCand].Prov;
                txtOutScoreComune.Text = outVerifyFree.Output[currCand].ScoreComune.ToString();
            }
            Cursor = Cursors.Default;
        }

        private void btnMovePrev_Click(object sender, EventArgs e)
        {
            // dettaglio del successivo candidato se esiste
            if (outVerifyFree.Output.Count > 0)
            {
                currCand -=1;
                txtOutCap.Text = outVerifyFree.Output[currCand].Cap;
                txtOutComune.Text = outVerifyFree.Output[currCand].Comune;
                txtOutFrazione.Text = outVerifyFree.Output[currCand].Frazione;
                txtOutProvincia.Text = outVerifyFree.Output[currCand].Prov;
                txtOutScoreComune.Text = outVerifyFree.Output[currCand].ScoreComune.ToString();
            }
        }

        private void btnMoveNext_Click(object sender, EventArgs e)
        {
            // dettagli del precedente candidato se esiste
            if (currCand + 1 < outVerifyFree.Output.Count)
            {
                currCand += 1;
                txtOutCap.Text = outVerifyFree.Output[currCand].Cap;
                txtOutComune.Text = outVerifyFree.Output[currCand].Comune;
                txtOutFrazione.Text = outVerifyFree.Output[currCand].Frazione;
                txtOutProvincia.Text = outVerifyFree.Output[currCand].Prov;
                txtOutScoreComune.Text = outVerifyFree.Output[currCand].ScoreComune.ToString();
            }
        }
    }
}
