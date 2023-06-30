using System;
using System.IO;

class Program
{
    static string[] produtos; // Array de strings para armazenar os nomes dos produtos
    static int[] estoque; // Array de inteiros para armazenar o estoque dos produtos
    static int[,] vendas; // Matriz de inteiros para armazenar as vendas dos produtos em cada dia

    static void Main()
    {
        int opcao;
        do
        {
            Console.WriteLine("1 – Importar arquivo de produtos"); // Opção para importar arquivo de produtos
            Console.WriteLine("2 – Registrar venda"); // Opção para registrar uma venda
            Console.WriteLine("3 – Relatório de vendas"); // Opção para exibir o relatório de vendas
            Console.WriteLine("4 – Relatório de estoque"); // Opção para exibir o relatório de estoque
            Console.WriteLine("5 – Criar arquivo de vendas"); // Opção para criar um arquivo de vendas
            Console.WriteLine("6 - Sair"); // Opção para sair do programa
            Console.WriteLine();
            Console.Write("Escolha uma opção: ");
            opcao = int.Parse(Console.ReadLine());
            Console.WriteLine();

            switch (opcao)
            {
                case 1:
                    ImportarArquivoProdutos(); // Chama o método para importar o arquivo de produtos
                    break;
                case 2:
                    RegistrarVenda(); // Chama o método para registrar uma venda
                    break;
                case 3:
                    RelatorioVendas(); // Chama o método para exibir o relatório de vendas
                    break;
                case 4:
                    RelatorioEstoque(); // Chama o método para exibir o relatório de estoque
                    break;
                case 5:
                    CriarArquivoVendas(); // Chama o método para criar um arquivo de vendas
                    break;
                case 6:
                    Console.WriteLine("Encerrando o programa...");
                    break;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }

            Console.WriteLine();
        } while (opcao != 6);
    }

    static void ImportarArquivoProdutos()
    {
        Console.Write("Digite o caminho do arquivo de produtos: ");
        string arquivoProdutos = Console.ReadLine(); // Lê o caminho do arquivo de produtos fornecido pelo usuário

        try
        {
            string[] linhas = File.ReadAllLines(arquivoProdutos); // Lê todas as linhas do arquivo de produtos

            int numProdutos = linhas.Length / 2; // Calcula o número de produtos com base na quantidade de linhas
            produtos = new string[numProdutos]; // Inicializa o array de produtos com o tamanho correto
            estoque = new int[numProdutos]; // Inicializa o array de estoque com o tamanho correto

            for (int i = 0; i < linhas.Length; i += 2)
            {
                produtos[i / 2] = linhas[i]; // Armazena o nome do produto na posição correta do array
                estoque[i / 2] = int.Parse(linhas[i + 1]); // Armazena a quantidade de estoque na posição correta do array
            }

            vendas = new int[numProdutos, 30]; // Inicializa a matriz de vendas com o número de produtos e 30 dias

            Console.WriteLine("Arquivo de produtos importado com sucesso!");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Arquivo não encontrado.");
        }
        catch (Exception)
        {
            Console.WriteLine("Ocorreu um erro ao ler o arquivo de produtos.");
        }
    }

    static void RegistrarVenda()
    {
        if (produtos == null)
        {
            Console.WriteLine("Importe o arquivo de produtos primeiro.");
            return;
        }

        Console.Write("Digite o número do produto: ");
        int numProduto = int.Parse(Console.ReadLine()) - 1; // Lê o número do produto fornecido pelo usuário e subtrai 1 para corresponder ao índice do array

        if (numProduto < 0 || numProduto >= produtos.Length)
        {
            Console.WriteLine("Produto inválido.");
            return;
        }

        Console.Write("Digite o dia do mês: ");
        int dia = int.Parse(Console.ReadLine()) - 1; // Lê o dia do mês fornecido pelo usuário e subtrai 1 para corresponder ao índice da matriz

        if (dia < 0 || dia >= 30)
        {
            Console.WriteLine("Dia inválido.");
            return;
        }

        Console.Write("Digite a quantidade vendida: ");
        int quantidade = int.Parse(Console.ReadLine()); // Lê a quantidade vendida fornecida pelo usuário

        if (quantidade <= 0)
        {
            Console.WriteLine("Quantidade inválida.");
            return;
        }

        if (quantidade > estoque[numProduto])
        {
            Console.WriteLine("Quantidade em estoque insuficiente.");
            return;
        }

        estoque[numProduto] -= quantidade; // Subtrai a quantidade vendida do estoque do produto
        vendas[numProduto, dia] += quantidade; // Adiciona a quantidade vendida na matriz de vendas

        Console.WriteLine("Venda registrada com sucesso!");
    }

    static void RelatorioVendas()
    {
        if (produtos == null)
        {
            Console.WriteLine("Importe o arquivo de produtos primeiro.");
            return;
        }

        Console.WriteLine("Relatório de vendas:");
        Console.WriteLine();
        Console.WriteLine("Produto\t\tDia\tQuantidade");

        for (int i = 0; i < produtos.Length; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                Console.WriteLine("{0}\t\t{1}\t{2}", produtos[i], j + 1, vendas[i, j]); // Exibe as informações de vendas para cada produto e dia
            }
        }
    }

    static void RelatorioEstoque()
    {
        if (produtos == null)
        {
            Console.WriteLine("Importe o arquivo de produtos primeiro.");
            return;
        }

        Console.WriteLine("Relatório de estoque:");
        Console.WriteLine();
        Console.WriteLine("Produto\t\tEstoque");

        for (int i = 0; i < produtos.Length; i++)
        {
            Console.WriteLine("{0}\t\t{1}", produtos[i], estoque[i]); // Exibe as informações de estoque para cada produto
        }
    }

    static void CriarArquivoVendas()
    {
        if (produtos == null)
        {
            Console.WriteLine("Importe o arquivo de produtos primeiro.");
            return;
        }

        Console.Write("Digite o nome do arquivo de vendas: ");
        string arquivoVendas = Console.ReadLine(); // Lê o nome do arquivo de vendas fornecido pelo usuário

        try
        {
            using (StreamWriter writer = new StreamWriter(arquivoVendas)) // Cria um StreamWriter para escrever no arquivo de vendas
            {
                for (int i = 0; i < produtos.Length; i++)
                {
                    writer.WriteLine(produtos[i]); // Escreve o nome do produto no arquivo
                    int totalVendas = 0;

                    for (int j = 0; j < 30; j++)
                    {
                        totalVendas += vendas[i, j]; // Calcula o total de vendas para o produto em todos os dias
                    }

                    writer.WriteLine(totalVendas); // Escreve o total de vendas no arquivo
                }
            }

            Console.WriteLine("Arquivo de vendas criado com sucesso!");
        }
        catch (Exception)
        {
            Console.WriteLine("Ocorreu um erro ao criar o arquivo de vendas.");
        }
    }
}
