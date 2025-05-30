pipeline {
    agent any

    environment {
        PATH = "$PATH:$HOME/.dotnet/tools"
    }

    stages {
        stage('Instalar ReportGenerator') {
            steps {
                script {
                    sh "dotnet tool install -g dotnet-reportgenerator-globaltool"
                }
            }
        }
        stage('Checkout') {
            steps {
                script {
                    checkout scm
                }
            }
        }
        stage('Parar Serviços') {
            steps {
                script {
                    sh "docker-compose down"
                }
            }
        }
        stage('Construir e Subir Serviços') {
            steps {
                script {
                    sh "docker-compose up -d --build"
                }
            }
        }
        stage('Executar Testes e Coletar Cobertura') {
            steps {
                script {
                    // Executa os testes e coleta a cobertura de código
                    sh "dotnet test --collect:\"XPlat Code Coverage\" --logger trx --results-directory ./TestResults"
                    
                    // Converte o relatório de cobertura para o formato Cobertura
                    sh "~/.dotnet/tools/reportgenerator -reports:TestResults/**/coverage.cobertura.xml -targetdir:TestResults/CoverageReport -reporttypes:Cobertura"
                    
                    // Verifica o conteúdo do relatório de cobertura
                    sh "cat TestResults/CoverageReport/Cobertura.xml"
                    
                    // Verifica se o arquivo de cobertura foi gerado corretamente
                    sh "ls -la TestResults/CoverageReport"
                    sh "test -f TestResults/CoverageReport/Cobertura.xml"
                }
            }
        }
        //stage('SonarQube Analysis') {
        //    steps {
        //        script {
        //            withSonarQubeEnv('SonarQube Server') {
        //                sh "dotnet sonarscanner begin /k:\"RGR-TRANSPORTE\" /d:sonar.host.url=\"http://66.135.11.124:9000\" /d:sonar.cs.opencover.reportsPaths=\"TestResults/CoverageReport/Cobertura.xml\" /d:sonar.cs.vstest.reportsPaths=\"TestResults/*.trx\" /d:sonar.verbose=true /d:sonar.scanner.scanAll=false"
        //                sh "dotnet build"
        //                sh "dotnet sonarscanner end"
        //            }
        //        }
        //    }
        //}
        stage('Limpar Imagens Docker') {
            steps {
                script {
                    sh "docker image prune -f"
                }
            }
        }
        stage('Limpar Recursos Docker') {
            steps {
                script {
                    sh "docker network prune -f"
                    sh "docker volume prune -f"
                }
            }
        }
    }
    post {
        always {
            cleanWs() // Limpa o workspace ao final, independentemente do resultado
        }
    }
}
