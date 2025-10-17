using System;
using System.Reflection;
using DesafioPOO.Models;



// Desafio POO - Corretora Imobiliária
List<Imovel> imoveis = new DataBase().LerImoveis(); // Le dados do bando de dados
 
// Menu interativo
int opcao = -1; // Cria variável inteira, iniciada em -1, para não gerar erro.
bool verifica = true; // variavel booleana para validar o While

    while (verifica)
    {
        Console.Clear(); // limpa a tela quando ao menu
        Console.WriteLine("\n ### CORRETORA iMOBILIÁRIA ### \n");
        Console.WriteLine("Escolha uma das opções abaixo: \n");
        Console.WriteLine("1 - Cadastrar Imóvel");
        Console.WriteLine("2 - Listar imóveis");
        Console.WriteLine("3 - Alugar/Disponibilizar imóvel");
        Console.WriteLine("4 - Calcular Aluguel");
        Console.WriteLine("5 - Deletar Imóveis");
        Console.WriteLine("0 - Sair/Encerrar Programa");
        string entrada = Console.ReadLine();

        if (!int.TryParse(entrada, out opcao))
        { // validando se o usuário digitou apenas números
            Console.WriteLine(" ATENÇÃO - OPÇÃO INVÁLIDA! Digite apenas números de 0 a 5. ");
            EsperarRetornoMenu();
            continue; // volta ao menu
        }
        switch (opcao)
        {
            case 1:
            CadastrarImovel();
                break;
            case 2:
            ListarImoveis();
                break;
        case 3:
            AlterarStatusImovel();
                break;
            case 4:
            CalcularValorAluguel();
                break;
            case 5:
            DeletarImovel();
                break;
            case 0:
                Console.WriteLine("\n Encerrando...");
                verifica = false;
                break;
            default:
                Console.WriteLine("\n Opção inexistente!");
                EsperarRetornoMenu();
                break;
        }
    }


// Auxilia a voltar ao menu quando usuário digita qualquer opção que não seja número
static void EsperarRetornoMenu()
{
    Console.WriteLine("\nPressione ENTER para voltar ao menu...");
    Console.ReadLine();
}

void CadastrarImovel()
{
    // Solicita entrada de dados
    Console.WriteLine("\n1 - Casa\n2 - Apartamento");
    Console.WriteLine("Sua escolha é: ");
    string tipo = Console.ReadLine();

    Console.WriteLine("Digite o lagradouro do imovel (rua, avenida etc): ");
    string endereco = Console.ReadLine();

    Console.WriteLine("Digite o numero do imóvel(apenas numeros): ");
    int numero = int.Parse(Console.ReadLine());

    Console.WriteLine("Digite o nome do proprietário do imóvel: ");
    string nome = Console.ReadLine();

    Console.WriteLine("Digite o telefone do proprietário: ");
    string telefone = Console.ReadLine();

    Console.WriteLine("Digite o CPF do proprietário: ");
    string cpf = Console.ReadLine();

    var proprietario = new Proprietario(nome, telefone, cpf); // Aglomera os dados do ao proprietário

    Imovel novoImovel = null; // inicia com valor nulo
    // Direciona qual tipo de imovel
    if (tipo == "1") 
    {
        novoImovel = new Casa(endereco, numero, proprietario);
    }
    else if (tipo == "2")
    {
        novoImovel = new Apartamento(endereco, numero, proprietario);
    }
    else
    {
        Console.WriteLine("Tipo inválido!"); // Erro caso não digitar uma das opções apresentadas
        EsperarRetornoMenu();
        return;
    }

    imoveis.Add(novoImovel); // Cria novo imóvel

    // SALVAR NO BANCO
    var db = new DataBase();
    db.SalvarImovel(novoImovel);

    Console.WriteLine("Imóvel cadastrado e sincronizado com o banco de dados!"); // Finaliza cadastro
    Console.ReadKey();
}

void ListarImoveis() // Lista os imóveis
{
    Console.WriteLine("## LISTA DE IMÓVEIS ##");
    if (imoveis.Count == 0)
    {
        Console.WriteLine("Nenhum imóvel cadastrado.");
        Console.ReadKey(); // espera o usuário antes de voltar ao menu
        return;
    }
    int i = 1; 
    foreach (var imovel in imoveis) // Estrutura do imóvel começando por numero 1 ao invés de zero
    {
        Console.WriteLine($"{i++} - {imovel.GetEndereco()}, Número {imovel.GetNumero()}");
        Console.WriteLine(imovel.EstaAlugado());
        Console.WriteLine(imovel.ContatoProprietario());
        Console.WriteLine("----------------------------");
    }
    Console.ReadKey();
}

void AlterarStatusImovel() // Funçao para ALUGAR ou DISPONIBILIZAR o Imóvel
{
    if (imoveis.Count == 0) { // verifica se existe imóveis cadastrados - evitar erro no código
        Console.WriteLine("Nenhum imóvel cadastrado.");
        Console.ReadKey(); // espera o usuário
        return;           
    }

    Console.WriteLine("## ALUGAR / DISPONIBILIZAR IMÓVEL ##");
    int i = 1;
    foreach (var imovel in imoveis) { // lista os imóveis iniciando por 1
        Console.WriteLine($"{i++} - {imovel.GetEndereco()}, Nº {imovel.GetNumero()} - {imovel.EstaAlugado()}");
    }

    Console.Write("Digite o número do imóvel que deseja alterar o status: "); //
    int indice = int.Parse(Console.ReadLine()) - 1; // le o usuário, converte para inteiro e diminui 1 no indice, ja que acredcentei para não iniciar em zero

    if (indice >= 0 && indice < imoveis.Count) { // Verifica se exite o imóvel 
        var imovel = imoveis[indice];
        imovel.SetAlugado(!imovel.GetAlugado()); // alterna o status
        Console.WriteLine(imovel.EstaAlugado()); // Imprimi a alteração
    }
    else {
        Console.WriteLine("Imóvel não encontrado!"); // Imprimi mensagem de erro quando ão identifica o imóvel
    }
    Console.ReadKey();
}

void CalcularValorAluguel()
{
    Console.WriteLine("## CALCULAR ALUGUEL ##");
    if (imoveis.Count == 0) // verifica se há imóvel cadastrado
    {
        Console.WriteLine("Nenhum imóvel cadastrado.");
        Console.ReadKey();
        return;
    }
    int i = 1;
    foreach (var imovel in imoveis)
    {
        Console.WriteLine($"{i++} - {imovel.GetEndereco()}, Nº {imovel.GetNumero()}"); // adiciona 1 ao indice para não iniciar em zero
    }

    Console.Write("Digite o número do imóvel: ");
    int indice = int.Parse(Console.ReadLine()) - 1; // ver linha 155

    if (indice >= 0 && indice < imoveis.Count) // Verifica se há imovel cadastrado e se indice maior que numero de imoveis cadasdtrado
    {
        Console.Write("Informe o valor mensal do aluguel: "); // Solicita vlor aluguel
        int valor = int.Parse(Console.ReadLine());

        Console.Write("Informe a quantidade de meses: "); // solicita numero de meses
        int meses = int.Parse(Console.ReadLine());

        int total = imoveis[indice].CalcularAluguel(valor, meses); // calcula valor aluguel x meses
        Console.WriteLine($"Valor total do aluguel: R$ {total}"); // imprimi resultado
    }
    else
    {
        Console.WriteLine("Imóvel não encontrado!"); // mensagem de erro
    }
    Console.ReadKey();
}

void DeletarImovel()
{
    if (imoveis.Count == 0)
    {
        Console.WriteLine("Nenhum imóvel cadastrado.");
        Console.ReadKey();
        return;
    }

    Console.WriteLine("## DELETAR IMÓVEL ##");
    int i = 1;
    foreach (var item in imoveis)
    {
        Console.WriteLine($"{i++} - {item.GetEndereco()}, Nº {item.GetNumero()}");
    }

    Console.Write("Digite o número do imóvel que deseja deletar: ");
    int indiceSelecionado = int.Parse(Console.ReadLine()) - 1; 

    if (indiceSelecionado >= 0 && indiceSelecionado < imoveis.Count) // verifica o imovel para deletar é maior que No de imoveis cadastrados
    {
        Imovel imovelSelecionado = imoveis[indiceSelecionado];

        // Apagar do banco
        new DataBase().DeletarImovelDoBanco(imovelSelecionado);

        // Remover da lista
        imoveis.RemoveAt(indiceSelecionado);

        Console.WriteLine("Imóvel removido com sucesso!");
    }
    else
    {
        Console.WriteLine("Imóvel não encontrado!"); // mendagem de erro
    }
    Console.ReadKey();
}
