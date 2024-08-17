pipeline {
    agent any 

    environment {
        // Adiciona o caminho para ferramentas .NET globais ao PATH
        PATH = "$PATH:$HOME/.dotnet/tools"
    }

    stages {
        stage('Checkout') {
            steps {
                script {
                    // Realiza o checkout do código fonte
                    checkout scm 
                }
            }
        }
        stage('Parar Serviços') {
            steps {
                script {
                    // Para os serviços Docker que estão rodando
                    sh "docker-compose down"
                }
            }
        }
        stage('Construir e Subir Serviços') {
            steps {
                script {
                    // Constrói e sobe os serviços Docker
                    sh "docker-compose up -d --build"
                }
            }
        }
        stage('Executar Testes e Coletar Cobertura') {
            steps {
                script {
                    // Executa os testes e coleta cobertura de código
                    sh "dotnet test --collect:\"XPlat Code Coverage\" --results-directory ./TestResults"
                }
            }
        }
        stage('SonarQube Analysis') {
            steps {
                script {
                    withSonarQubeEnv('SonarQube Server') {
                        // Inicia o scanner do SonarQube
                        sh "dotnet sonarscanner begin /k:\"RGR-TRANSPORTE\" /d:sonar.host.url=\"http://66.135.11.124:9000\" /d:sonar.cs.opencover.reportsPaths=\"TestResults/coverage.cobertura.xml\""
                        // Compila o código após a configuração do SonarQube
                        sh "dotnet build"
                        // Finaliza o scanner do SonarQube
                        sh "dotnet sonarscanner end"
                    }
                }
            }
        }
        stage('Limpar Imagens Docker') {
            steps {
                script {
                    // Limpa imagens Docker não utilizadas
                    sh "docker image prune -f"
                }
            }
        }
        stage('Limpar Recursos Docker') {
            steps {
                script {
                    // Limpa redes e volumes Docker não utilizados
                    sh "docker network prune -f"
                    sh "docker volume prune -f"
                }
            }
        }
    }
    post {
        always {
            // Limpa o workspace após a execução do pipeline
            cleanWs()
        }
    }
}
