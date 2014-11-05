﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using net.r_eg.vsSBE;
using net.r_eg.vsSBE.Exceptions;
using net.r_eg.vsSBE.SBEScripts;
using net.r_eg.vsSBE.SBEScripts.Components;
using net.r_eg.vsSBE.SBEScripts.Exceptions;

namespace vsSBETest
{
    /// <summary>
    ///This is a test class for UserVariableComponentTest and is intended
    ///to contain all UserVariableComponentTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UserVariableComponentTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        /// <summary>
        ///A test for parse
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SyntaxIncorrectException))]
        public void parseTest()
        {
            UserVariableComponent target = new UserVariableComponent((IEnvironment)null, new UserVariable());
            target.parse("#[var name = value]");
        }

        /// <summary>
        ///A test for parse
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SyntaxIncorrectException))]
        public void parseTest2()
        {
            UserVariableComponent target = new UserVariableComponent((IEnvironment)null, new UserVariable());
            target.parse("var name = value");
        }

        /// <summary>
        ///A test for parse
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(NotFoundException))]
        public void parseTest3()
        {
            UserVariableComponent target = new UserVariableComponent((IEnvironment)null, new UserVariable());
            target.parse("[var name]");
        }

        /// <summary>
        ///A test for parse
        ///</summary>
        [TestMethod()]
        public void parseTest4()
        {
            UserVariableComponentAccessor target = new UserVariableComponentAccessor((IEnvironment)null, new UserVariable());
            Assert.AreEqual(String.Empty, target.parse("[var name = value]"));
            Assert.AreEqual("[E1:value]", target.parse("[var name]"));
        }

        /// <summary>
        ///A test for parse
        ///</summary>
        [TestMethod()]
        public void parseTest5()
        {
            UserVariableComponentAccessor target = new UserVariableComponentAccessor((IEnvironment)null, new UserVariable());
            Assert.AreEqual(String.Empty, target.parse("[var name = line1 \n line2]"));
            Assert.AreEqual("[E1:line1 \n line2]", target.parse("[var name]"));
        }

        /// <summary>
        ///A test for parse
        ///</summary>
        [TestMethod()]
        public void parseTest6()
        {
            UserVariableComponentAccessor target = new UserVariableComponentAccessor((IEnvironment)null, new UserVariable());
            Assert.AreEqual(String.Empty, target.parse("[var name = value]"));
            Assert.AreEqual(String.Empty, target.parse("[var name = value2]"));
            Assert.AreEqual("[E1:value2]", target.parse("[var name]"));
        }

        private class UserVariableComponentAccessor: UserVariableComponent
        {
            public UserVariableComponentAccessor(IEnvironment env, IUserVariable uvariable): base(env, uvariable)
            {

            }

            protected override void evaluate(string name, string project = null)
            {
                uvariable.evaluate(name, project, new Evaluator1(), true);
            }
        }

        private class Evaluator1: IEvaluator
        {
            public string evaluate(string data)
            {
                return String.Format("[E1:{0}]", data);
            }
        }
    }
}
