﻿using PlataformaEducacional.Core.DomainObjects;

namespace PlataformaEducacional.Aluno.Domain
{
    public class Aluno : EntityBase, IAggregateRoot
    {
        public Aluno(string nomeCompleto, string email, DateTime dataNascimento)
        {
            NomeCompleto = nomeCompleto;
            Email = email;
            DataNascimento = dataNascimento;

            Validar();
        }

        public Aluno(Guid id, string nomeCompleto, string email, DateTime dataNascimento)
        {
            Id = id;
            NomeCompleto = nomeCompleto;
            Email = email;
            DataNascimento = dataNascimento;
            DataCadastro = DateTime.Now;

            Validar();
        }

        public string NomeCompleto { get; private set; }
        public string Email { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public DateTime DataCadastro { get; private set; }

        public virtual ICollection<Matricula>? Matriculas { get; private set; }
        public virtual ICollection<Certificado>? Certificados { get; private set; }


        public void Validar()
        {
            Validacoes.ValidarSeVazio(NomeCompleto, "O campo Nome do aluno não pode estar vazio");
            Validacoes.ValidarSeVazio(Email, "O campo Email do aluno não pode estar vazio");
            Validacoes.ValidarSeNulo(DataNascimento, "O campo Data de nascimento do aluno não pode estar vazio");
        }

        public void AdicionarMatricula(Matricula matricula)
        {
            if(Matriculas == null)
                Matriculas = new List<Matricula>();

            Matriculas.Add(matricula);
        }

        public void AdicionarCertificado(Certificado certificado)
        {
            if (Certificados == null)
                Certificados = new List<Certificado>();

            Certificados.Add(certificado);
        }
    }
}
