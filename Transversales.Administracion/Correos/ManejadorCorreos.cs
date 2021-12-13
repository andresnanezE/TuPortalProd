// ***********************************************************************
// Ensamblado         		: Emermedica Sitio Empresarial - Infraestructura.Transversales.Administracion - ManejadorCorreos.cs
// Autor					: Alex Mauricio Palacios Caicedo
// Creado          			: 16/02/2015
//
// Ultima Modificación por 	: Alex Mauricio Palacios Caicedo
// Ultima Modificación en 	: 16/02/2015
// ***********************************************************************
// <copyright file="ManejadorCorreos.cs" Compañia="SCI Software">
//     Copyright (c) SCI Software. All rights reserved.
// </copyright>
// ***********************************************************************

using ActiveUp.Net.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Transversales.Administracion.Correos
{
    public class ManejadorCorreos : IManejadorCorreos
    {
        private readonly bool _enableSsl;
        private readonly string _host;
        private readonly string _password;
        private readonly int _port;
        private readonly string _userName;
        private IEnumerable<string> _adjuntos;
        private string _asunto;
        private IEnumerable<string> _destinos;
        private string _mensaje;

        public ManejadorCorreos()
        {
            _host = ConfigurationManager.AppSettings["Host"];
            _password = ConfigurationManager.AppSettings["Password"];
            _port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
            _userName = ConfigurationManager.AppSettings["Username"];
            _enableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"]);
        }

        public IEnumerable<string> Destinatarios
        {
            get { return _destinos; }
            set { _destinos = value; }
        }

        public IEnumerable<string> Adjuntos
        {
            get { return _adjuntos; }
            set { _adjuntos = value; }
        }

        public string Mensaje
        {
            get { return _mensaje; }
            set { _mensaje = value; }
        }

        public string Asunto
        {
            get { return _asunto; }
            set { _asunto = value; }
        }

        public void Enviar()
        {
            var servers = new ServerCollection();
            var server = new Server();
            var message = new Message();
            var mimeBody = new MimeBody(BodyFormat.Html);
            var destinos = new AddressCollection();

            server.Host = _host;
            server.Password = _password;
            server.Port = _port;
            server.Username = _userName;
            server.ServerEncryptionType = _enableSsl ? EncryptionType.SSL : EncryptionType.None;

            servers.Add(server);

            if (_destinos != null)
            {
                foreach (string destino in _destinos)
                {
                    destinos.Add(new Address(destino));
                }
            }

            if (_adjuntos != null)
            {
                foreach (string adjunto in _adjuntos)
                {
                    message.Attachments.Add(adjunto, false);
                }
            }

            mimeBody.Text = _mensaje;
            message.BodyHtml = mimeBody;
            message.Date = DateTime.Now;
            message.From = new Address(_userName);
            message.Priority = MessagePriority.Normal;
            message.To = destinos;
            message.Subject = _asunto;

            SmtpClient.Send(message, servers);
        }
    }
}