# FIAP.PosTech.ArqSistemas.NotificationWS

Serviço de background (**Worker Service**) desenvolvido em **.NET 8** responsável pelo consumo de eventos de integração e disparo automatizado de comunicações e e-mails transacionais (boas-vindas e confirmações de compra) para os usuários do ecossistema.

O componente atua de forma reativa e assíncrona utilizando o **Apache Kafka** para escutar atualizações cadastrais e liquidações financeiras, processando os dados e acionando o motor de e-mail via **MailKit**.

---

## 🛠️ Tecnologias e Frameworks

* **Runtime:** .NET 8.0 (BackgroundService)
* **Mensageria (Consumo):** Confluent Kafka Client (Event-Driven Consumers - `UserCreated` e `PaymentProcessed`)
* **Motor de Envio de E-mail:** MailKit & MimeKit (Protocolo SMTP com suporte a TLS/SSL)
* **Containers & Orquestração:** Docker & Kubernetes

---

## 🎯 Escopo e Funcionamento Interno

Este Worker opera focado na camada de notificação e engajamento do ecossistema, executando loops paralelos de consumo assíncrono divididos em dois fluxos principais:

1. **Assinatura e Loop de Escuta (`KafkaConsumerWorker`):** O serviço de background centraliza e inicializa simultaneamente os consumidores através de tarefas gerenciadas (`Task.WhenAll`), escutando os tópicos configurados no Kafka.
2. **Fluxo de Novo Usuário (`UserEventConsumer`):**
   * Captura mensagens do tópico de criação de contas (`TopicNameUserCreated`).
   * Desserializa o payload para o formato `UserCreatedEventDto`.
   * Invoca o `EmailService` para disparar um e-mail de boas-vindas personalizado com visual *gamer*, confirmando o acesso ao lobby da plataforma.
3. **Fluxo de Compra Confirmada (`PaymentProcessedEventConsumer`):**
   * Captura mensagens do tópico de pagamentos liquidados com sucesso (`TopicNamePaymentProcessed`).
   * Desserializa o payload para o formato `PaymentProcessedCreatedEvent`.
   * Aciona o `EmailService` para enviar uma notificação de parabéns pela aquisição, informando que o jogo já está disponível na biblioteca do usuário.
4. **Motor de Disparo (`EmailServiceMailKit`):** Centraliza a montagem de e-mails profissionais utilizando templates HTML modernos, realizando a conexão, autenticação segura e transmissão limpa junto ao servidor SMTP configurado.

---

### Repositório do Ecossistema
Você precisará clonar o seguintes repositório do projeto:

| Repositório | Link para Clone |
| :--- | :--- |
| **Notification WS** | `https://github.com/rodrigosiqsilva/FIAP.PosTech.ArqSistemas.Notification.git` |

### 📂 Estrutura de Pastas Obrigatória
Para que os arquivos de orquestração local (Docker Compose) referenciem os projetos corretamente, você **deve** respeitar a seguinte estrutura de diretórios no seu disco:

Veja um exemplo através da imagem: https://github.com/rodrigosiqsilva/FIAP.PosTech.ArqSistemas.Orchestrator/blob/main/Estrututa%20pastas.png

```text
C:\Sistemas\FIAP\     
├── FIAP.PosTech.ArqSistemas.Catalog/  
├── FIAP.PosTech.ArqSistemas.User/
├── FIAP.PosTech.ArqSistemas.Notification/ <- (Arquivos desse repositório mencionados aqui)
└── FIAP.PosTech.ArqSistemas.Payments/