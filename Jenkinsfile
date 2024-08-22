pipeline {
    agent any 

    stages {
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
                    // Habilita o BuildKit, que utiliza o buildx
                    sh "DOCKER_BUILDKIT=1 docker-compose up -d --build"
                }
            }
        }
        stage('Instalar Dependências') {
            steps {
                script {
                    sh "npm install"
                }
            }
        }
        stage('Executar Testes e Coletar Cobertura') {
            steps {
                script {
                    sh "npm test -- --coverage"
                }
            }
        }
        stage('SonarQube Analysis') {
            steps {
                script {
                    withSonarQubeEnv('SonarQube Server') {
                        sh '''
                            sonar-scanner \
                            -Dsonar.projectKey=rgrfront \
                            -Dsonar.sources=. \
                            -Dsonar.host.url=http://66.135.11.124:9000
                        '''
                    }
                }
            }
        }
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
            cleanWs()
        }
    }
}
