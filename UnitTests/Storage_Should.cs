using AutoFixture.Xunit2;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Moq;
using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests;
public class Storage_Should
{
	[Theory, AutoDomainData]
	public void CreateNewStoragePartion_when_PassingMidnigth(
		[Frozen] Mock<ILogStorage> mock)
	{
		mock.Verify(x => x.CreateStoragePartition())
	}
}

