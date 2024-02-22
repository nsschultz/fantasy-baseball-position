pipeline {
  agent { label 'builder' }
  environment {
    def ver = readFile file: 'version.txt'
    VERSION_NUMBER = "${ver}"
    IMAGE_VERSION = "${GIT_BRANCH == "main" ? VERSION_NUMBER : VERSION_NUMBER+"-"+GIT_BRANCH}"
    DOCKER_HUB = credentials("dockerhub-creds")
  }
  stages {
    stage ('build and scan') {
      steps { script { sh  """
        #!/bin/bash
        sh .docker-compose/scripts/ci.sh
      """ } } 
    }
    stage('build and publish release') { 
      steps { script { sh  """
        #!/bin/bash
        docker build -t nschultz/fantasy-baseball-position:${IMAGE_VERSION} .
        docker login -u ${DOCKER_HUB_USR} -p ${DOCKER_HUB_PSW}
        docker push nschultz/fantasy-baseball-position:${IMAGE_VERSION}
        docker logout
      """ } } 
    }
    stage('deploy') { 
      when { branch 'main' }
      agent { label 'manager' }
      steps { script { sh """
        #!/bin/bash
        sed -i "s/{{version}}/${VERSION_NUMBER}/g" ./.deploy/position-deployment.yaml
        kubectl apply -f ./.deploy/position-database-deployment.yaml
        kubectl apply -f ./.deploy/position-database-service.yaml
        kubectl apply -f ./.deploy/position-deployment.yaml
        kubectl apply -f ./.deploy/position-service.yaml
        kubectl apply -f ./.deploy/position-ingress.yaml
      """ } }
    }
  }
}