using System;
using System.Collections.Generic;


namespace DesvendandoMinimalAPI.Domain
{

    public class Veiculo
    {
        public int Id { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }
        public int Ano { get; set; }
        public string Cor { get; set; }
    }
}
