using System.Threading.Tasks;
using ZooM.Application.Events.Employees;
using ZooM.Application.Exceptions.Employee;
using ZooM.Application.Services;
using ZooM.Core.Repositories;

namespace ZooM.Application.Commands.Employees.Handlers
{
    class UpdateEmployeeHandler : ICommandHandler<UpdateEmployee>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMessageBroker _broker;
        public UpdateEmployeeHandler(IEmployeeRepository repository, IMessageBroker broker)
        {
            _repository = repository;
            _broker = broker;
        }

        public async Task HandleAsync(UpdateEmployee command)
        {
            var employee = await _repository.GetAsync(command.Id);

            if (employee is null)
            {
                throw new EmployeeDoesntExistException(command.Id);
            }

            if (command.Avatar != null)
            {
                employee.ChangeAvatar(command.Avatar);
            }

            if (command.Position != null)
            {
                employee.ChangePosition(command.Position.Value);
            }

            await _repository.UpdateAsync(employee);
           // await _broker.PublishAsync(new EmployeeUpdated(command.Id));
        }
    }
}
